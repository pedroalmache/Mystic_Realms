import {
  Component,
  DestroyRef,
  ElementRef,
  OnInit,
  computed,
  effect,
  inject,
  input,
  output,
  signal,
  viewChild,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';

import {
  ARMOR_BURST_MS,
  ATTACK_ANIMATION_MS,
  CARD_DISCARD_FADE_MS,
  CARD_DISCARD_HAND_FLIGHT_MS,
  CARD_ENTER_STAGGER_MS,
  DAMAGE_ANIMATION_MS,
  ELEMENT_ANIMATION_MS,
  DEFENSE_BURST_MS,
  DISCARD_PILE_PULSE_MS,
  ENTER_DURATION_MS,
  ENTER_STAGGER_MS,
  GAME_OVER_BUTTON_DELAY_MS,
  GAME_OVER_BUTTON_FADE_MS,
  GAME_OVER_FADE_MS,
  GAME_OVER_FINAL_DELAY_MS,
  GAME_OVER_TEXT_DELAY_MS,
  GAME_OVER_TEXT_FADE_MS,
  HEAL_ANIMATION_MS,
  POISON_ANIMATION_MS,
  MOMENTUM_REMOVE_FADE_MS,
  MOMENTUM_REMOVE_STAGGER_MS,
  MANA_ANIMATION_MS,
  LEAVE_DURATION_MS,
  MANA_STEP_MS,
  ROOM_WON_FADE_MS,
  ROOM_WON_FINAL_DELAY_MS,
  ROOM_WON_TEXT_DELAY_MS,
  ROOM_WON_TEXT_FADE_MS,
  ROOM_WON_TRANSITION_DELAY_MS,
  STRENGTH_BURST_MS,
  TURN_UPDATE_PEAK_MS,
  TURN_UPDATE_TAIL_MS,
} from '../../core/constants/animation.constants';
import { Assets } from '../../core/constants/assets.constants';
import { AuthService } from '../../core/auth/auth.service';
import {
  BoardDto,
  CardDto,
  EnemyDto,
  GameAction,
  GameActionType,
  Gender,
  InitiativeDto,
  InitiativeType,
  PlayerGameAction,
  PartyMemberDto,
  PlayCardRequest,
  RoomDto,
  SinglePlayerService,
} from '../../core/services/single-player.service';
import { CardComponent } from './card/card';
import { GameCharacterComponent } from '../../shared/game-character/game-character';
import { CardDisplayComponent } from '../../shared/card-display/card-display';
import { CardDescriptionHelper } from '../../core/utils/card-description.helper';
import { CardTooltipComponent } from '../../shared/card-tooltip/card-tooltip';

/** Animation state for an initiative bar entry. */
export type InitiativeAnimationState = 'entering' | 'active' | 'leaving';

/** An initiative entry augmented with a stable key and animation state. */
export interface InitiativeEntryState {
  entry: InitiativeDto;
  /** Stable identifier used for @for tracking. */
  key: string;
  animationState: InitiativeAnimationState;
}

/** Returns a stable string key for an initiative entry. */
function initiativeEntryKey(entry: InitiativeDto): string {
  if (entry.type === InitiativeType.Enemy && entry.enemyInstanceId) {
    return `enemy-${entry.enemyInstanceId}`;
  }
  return `player-${entry.name ?? 'unknown'}`;
}

/** State for a card that has been played but is awaiting target selection. */
interface PendingCardPlay {
  card: CardDto;
  remainingPartyTargets: number;
  remainingEnemyTargets: number;
  selectedPartyMemberInstanceIds: string[];
  selectedEnemyInstanceIds: string[];
}

interface BurstState {
  delta: number;
  token: number;
}

interface AttackAnimationState {
  token: number;
}

interface HpChangeAnimationState {
  delta: number;
  token: number;
}

interface ResourceAnimationState {
  delta: number | null;
  token: number;
}

type ArmorPresentation = 'physical' | 'magic';

/** Resolves after {@link ms} milliseconds. */
function delay(ms: number): Promise<void> {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

/** Fallback dead sprite used when enemy ID is missing. */
const FALLBACK_DEAD_ENEMY_SPRITE = 'assets/images/enemies/1/dead.webp';

@Component({
  selector: 'app-encounter',
  templateUrl: './encounter.html',
  styleUrl: './encounter.scss',
  imports: [GameCharacterComponent, CardComponent, CardDisplayComponent, CardTooltipComponent],
})
export class EncounterComponent {
  private readonly singlePlayerService = inject(SinglePlayerService);
  private readonly authService = inject(AuthService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);

  readonly actionLog = input<GameAction[]>([]);
  readonly gameId = input<number | null>(null);
  readonly isMyTurn = input<boolean>(false);
  readonly endTurn = output<void>();
  readonly actionsReceived = output<GameAction[]>();
  readonly roomWon = output<void>();

  protected readonly centerDropZoneEl = viewChild<ElementRef>('centerDropZone');
  protected readonly discardPileEl = viewChild<ElementRef>('discardPile');
  protected readonly isAnyCardDragging = signal<boolean>(false);
  /** The card currently pinned in the center zone (snapshotted at pin time). */
  protected readonly pinnedCard = signal<CardDto | null>(null);
  /** Composite slot id of the pinned card (partyMemberId + '-' + index). */
  protected readonly pinnedCardSlotId = signal<string | null>(null);

  /** Which pile panel is currently open: 'draw', 'discard', or null for neither. */
  protected readonly activePile = signal<'draw' | 'discard' | null>(null);
  protected readonly drawPileCards = signal<CardDto[]>([]);
  protected readonly discardPileCards = signal<CardDto[]>([]);

  // ── Sequential action-log processing ────────────────────────────────────
  private processedActionCount = 0;
  private processingActive = false;
  private readonly actionQueue: GameAction[] = [];
  private gameLoaded = false;
  private isDestroyed = false;

  // ── Display state driven by processed actions ────────────────────────────
  protected readonly displayTurn = signal<number>(0);
  protected readonly turnAnimating = signal<boolean>(false);
  protected readonly displayPartyMembers = signal<PartyMemberDto[]>([]);
  protected readonly displayEnemies = signal<EnemyDto[]>([]);
  protected readonly initiativeEntries = signal<InitiativeEntryState[]>([]);
  protected readonly displayHand = signal<CardDto[]>([]);
  protected readonly displayMana = signal<number>(0);
  protected readonly displayBoard = signal<BoardDto[]>([]);
  protected readonly hoveredTooltipCard = signal<CardDto | null>(null);
  /** Instance IDs of cards currently playing their enter animation. */
  protected readonly enteringCardInstanceIds = signal<Set<string>>(new Set());
  /** Instance IDs of cards currently fading out after a discard action. */
  protected readonly leavingCardInstanceIds = signal<Set<string>>(new Set());
  /** Instance IDs of cards currently flying into the discard pile due to DiscardHand. */
  protected readonly discardHandLeavingCardInstanceIds = signal<Set<string>>(new Set());
  /** Whether the draw pile is currently pulsing (a Draw action is in progress). */
  protected readonly drawPilePulsing = signal<boolean>(false);
  /** Whether the discard pile is currently pulsing (a DiscardHand action is in progress). */
  protected readonly discardPilePulsing = signal<boolean>(false);
  /** Active armor burst states keyed by party member identifier. */
  protected readonly partyMemberArmorBursts = signal<Record<string, BurstState>>({});
  /** Current armor presentation type keyed by party member identifier. */
  protected readonly partyMemberArmorPresentation = signal<Record<string, ArmorPresentation>>({});
  /** Active strength burst states keyed by party member identifier. */
  protected readonly partyMemberStrengthBursts = signal<Record<string, BurstState>>({});
  /** Active defense burst states keyed by party member identifier. */
  protected readonly partyMemberDefenseBursts = signal<Record<string, BurstState>>({});
  /** Active attack animation states keyed by party member identifier. */
  protected readonly partyMemberAttackState = signal<Record<string, AttackAnimationState>>({});
  /** Active attack animation states keyed by enemy instance ID. */
  protected readonly enemyAttackState = signal<Record<string, AttackAnimationState>>({});
  /** Active armor burst states keyed by enemy instance ID. */
  protected readonly enemyArmorBursts = signal<Record<string, BurstState>>({});
  /** Active strength burst states keyed by enemy instance ID. */
  protected readonly enemyStrengthBursts = signal<Record<string, BurstState>>({});
  /** Active defense burst states keyed by enemy instance ID. */
  protected readonly enemyDefenseBursts = signal<Record<string, BurstState>>({});
  /** Active speed burst states keyed by party member identifier. */
  protected readonly partyMemberSpeedBursts = signal<Record<string, BurstState>>({});
  /** Active speed burst states keyed by enemy instance ID. */
  protected readonly enemySpeedBursts = signal<Record<string, BurstState>>({});
  /** Active defend animation states (PhysicalDamage) keyed by enemy instance ID. */
  protected readonly enemyDefendState = signal<Record<string, HpChangeAnimationState>>({});
  /** Active defend animation states (PhysicalDamage) keyed by party member identifier. */
  protected readonly partyMemberDefendState = signal<Record<string, HpChangeAnimationState>>({});
  /** Active mana animation states keyed by party member identifier. */
  protected readonly partyMemberManaState = signal<Record<string, ResourceAnimationState>>({});
  /** Active healing animation states keyed by party member identifier. */
  protected readonly partyMemberHealState = signal<Record<string, HpChangeAnimationState>>({});
  /** Active healing animation states keyed by enemy instance ID. */
  protected readonly enemyHealState = signal<Record<string, HpChangeAnimationState>>({});
  /** Active poison animation states keyed by enemy instance ID. */
  protected readonly enemyPoisonState = signal<Record<string, HpChangeAnimationState>>({});
  /** Active poison animation states keyed by party member identifier. */
  protected readonly partyMemberPoisonState = signal<Record<string, HpChangeAnimationState>>({});
  /** Active daggers animation keys keyed by party member identifier. */
  protected readonly partyMemberDaggersAnimationState = signal<Record<string, number>>({});
  /** Active spirit animation keys keyed by party member identifier. */
  protected readonly partyMemberSpiritAnimationState = signal<Record<string, number>>({});
  /** Active element animation keys keyed by party member identifier. */
  protected readonly partyMemberElementAnimationState = signal<Record<string, number>>({});
  /** Active daggers animation keys keyed by enemy instance ID. */
  protected readonly enemyDaggersAnimationState = signal<Record<string, number>>({});
  /** Enemy instance IDs currently marked as dead. */
  protected readonly deadEnemyInstanceIds = computed<Set<string>>(() => {
    return new Set(this.displayEnemies().filter(e => this.isEnemyDead(e)).map(e => e.instanceId));
  });
  /** Party member identifiers currently marked as dead. */
  protected readonly deadPartyMemberIdentifiers = computed<Set<string>>(() => {
    return new Set(this.displayPartyMembers().filter(m => this.isPartyMemberDead(m)).map(m => m.identifier));
  });
  /** Whether the Game Over blackout overlay is active. */
  protected readonly gameOverActive = signal<boolean>(false);
  /** Whether the Game Over title has faded in. */
  protected readonly gameOverTextVisible = signal<boolean>(false);
  /** Whether the Continue button has faded in. */
  protected readonly gameOverButtonVisible = signal<boolean>(false);
  /** Whether the Room Won blackout overlay is active. */
  protected readonly roomWonActive = signal<boolean>(false);
  /** Whether the Room Won title has faded in. */
  protected readonly roomWonTextVisible = signal<boolean>(false);

  /** Pending card play awaiting target selection; null when no card is being targeted. */
  protected readonly pendingCardPlay = signal<PendingCardPlay | null>(null);
  /** Last card removed from hand while waiting for server play confirmation. */
  private readonly inFlightPlayedCard = signal<CardDto | null>(null);

  /** The mana value shown in the UI, animated 1-by-1 towards the real mana. */
  protected readonly displayedMana = signal<number>(0);
  /** True while the mana indicator is emphasizing an insufficient-mana event. */
  protected readonly manaAttentionActive = signal<boolean>(false);
  /** Incremented each time a new mana animation starts; used to cancel in-flight ones. */
  private manaAnimationVersion = 0;
  /** Timer id for clearing insufficient-mana attention animation state. */
  private manaAttentionTimeout: ReturnType<typeof setTimeout> | null = null;

  /** Incremented each time a new armor burst is triggered. */
  private armorBurstToken = 0;
  /** Incremented each time a new strength burst is triggered. */
  private strengthBurstToken = 0;
  /** Incremented each time a new defense burst is triggered. */
  private defenseBurstToken = 0;
  /** Incremented each time a new speed burst is triggered. */
  private speedBurstToken = 0;
  /** Incremented each time a new attack animation is triggered. */
  private attackAnimationToken = 0;
  /** Incremented each time a new defend/damage animation is triggered. */
  private defendAnimationToken = 0;
  /** Incremented each time a new healing animation is triggered. */
  private resourceGainAnimationToken = 0;

  constructor() {
    // When the action log grows, queue the new actions for sequential processing.
    effect(() => {
      const actionLog = this.actionLog();
      if (actionLog.length < this.processedActionCount) {
        // The game was reset – clear any in-flight state.
        this.processedActionCount = 0;
        this.actionQueue.length = 0;
        this.gameLoaded = false;
        this.partyMemberArmorPresentation.set({});
        this.gameOverActive.set(false);
        this.gameOverTextVisible.set(false);
        this.gameOverButtonVisible.set(false);
        this.roomWonActive.set(false);
        this.roomWonTextVisible.set(false);
        this.displayBoard.set([]);
      }
      const newActions = actionLog.slice(this.processedActionCount);
      if (newActions.length > 0) {
        this.actionQueue.push(...newActions);
        this.processedActionCount = actionLog.length;
        if (!this.processingActive) {
          void this.processQueue();
        }
      }
    });

    this.destroyRef.onDestroy(() => {
      this.isDestroyed = true;
      this.actionQueue.length = 0;
      this.clearManaAttentionTimeout();
    });
  }

  // ── Queue processing ─────────────────────────────────────────────────────

  /** Counts {@link displayedMana} towards {@link target} one step at a time. */
  private async animateMana(target: number): Promise<void> {
    const version = ++this.manaAnimationVersion;
    while (!this.isDestroyed) {
      const current = this.displayedMana();
      if (current === target) break;
      const next = current < target ? current + 1 : current - 1;
      this.displayedMana.set(next);
      if (next === target) break;
      await delay(MANA_STEP_MS);
      if (this.manaAnimationVersion !== version) return; // superseded
    }
  }

  private async processQueue(): Promise<void> {
    this.processingActive = true;
    while (this.actionQueue.length > 0 && !this.isDestroyed) {
      const action = this.actionQueue.shift()!;
      await this.processAction(action);
    }
    this.processingActive = false;
  }

  private async processAction(action: GameAction): Promise<void> {
    console.log(`[ActionLog] ${GameActionType[action.type]}`, action);
    switch (action.type) {
      case GameActionType.SetInitial:
        await this.processSetInitial(action);
        break;
      case GameActionType.UpdateTurn:
        await this.processUpdateTurn(action);
        break;
      case GameActionType.UpdateInitiative:
        await this.processUpdateInitiative(action);
        break;
      case GameActionType.Draw: {
        const player = action.players?.[0];
        const updatedHand: CardDto[] = player?.hand ?? action.card ?? [];
        if (player) {
          this.displayMana.set(player.mana);
          // Animate the mana counter towards the new value in parallel with the card draw.
          void this.animateMana(player.mana);
        }
        this.drawPilePulsing.set(true);
        await this.animateDrawIntoHand(updatedHand);
        if (!this.isDestroyed) {
          this.drawPilePulsing.set(false);
        }
        break;
      }
      case GameActionType.DiscardHand: {
        await this.processDiscardHand();
        break;
      }
      case GameActionType.Discard:
        await this.processDiscard(action);
        break;
      case GameActionType.Armor:
        await this.processArmor(action);
        break;
      case GameActionType.MagicArmor:
        await this.processMagicArmor(action);
        break;
      case GameActionType.Strength:
        await this.processStrength(action);
        break;
      case GameActionType.Defense:
        await this.processDefense(action);
        break;
      case GameActionType.Speed:
        await this.processSpeed(action);
        break;
      case GameActionType.Attack:
        await this.processAttack(action);
        break;
      case GameActionType.EnemyAttack:
        await this.processEnemyAttack(action);
        break;
      case GameActionType.PhysicalDamage:
      case GameActionType.MagicDamage:
        await this.processDamage(action);
        break;
      case GameActionType.EnemyArmor:
        await this.processEnemyArmor(action);
        break;
      case GameActionType.EnemyHealing:
        await this.processEnemyHealing(action);
        break;
      case GameActionType.Healing:
        await this.processHealing(action);
        break;
      case GameActionType.Mana:
        await this.processMana(action);
        break;
      case GameActionType.CardPlayed:
        this.processCardPlayed(action);
        break;
      case GameActionType.NotEnoughMana:
        this.processNotEnoughMana(action);
        break;
      case GameActionType.UpdateMana:
        this.processUpdateMana(action);
        break;
      case GameActionType.EnemyDied:
        this.processEnemyDied(action);
        break;
      case GameActionType.Died:
        this.processPartyMemberDied(action);
        break;
      case GameActionType.GameOver:
        await this.processGameOver(action);
        break;
      case GameActionType.RoomWon:
        await this.processRoomWon(action);
        break;
      case GameActionType.MomentumAdd:
        await this.processMomentumAdd(action);
        break;
      case GameActionType.MomentumRemove:
        await this.processMomentumRemove(action);
        break;
      case GameActionType.Poison:
        await this.processPoison(action);
        break;
      case GameActionType.PoisonConsumed:
        await this.processPoisonConsumed(action);
        break;
      case GameActionType.Daggers:
        await this.processDaggers(action);
        break;
      case GameActionType.Spirit:
        await this.processSpirit(action);
        break;
      case GameActionType.BoardAdd:
        await this.processBoardAdd(action);
        break;
      case GameActionType.Element:
        await this.processElement(action);
        break;
      default:
        console.log(`[ActionLog] No handler for action type ${action.type}.`);
    }
  }

  private async processBoardAdd(action: GameAction): Promise<void> {
    if (action.board) {
      this.displayBoard.set(action.board);
    }
  }

  private async processElement(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    if (updatedMembers.length === 0) {
      return;
    }

    this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

    const clearMembers: Array<{ identifier: string; token: number }> = [];

    this.partyMemberElementAnimationState.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        const token = ++this.resourceGainAnimationToken;
        next[member.identifier] = token;
        clearMembers.push({ identifier: member.identifier, token });
      }
      return next;
    });

    await delay(ELEMENT_ANIMATION_MS);
    if (this.isDestroyed) return;

    this.partyMemberElementAnimationState.update((current) => {
      const next = { ...current };
      for (const item of clearMembers) {
        if (next[item.identifier] === item.token) {
          delete next[item.identifier];
        }
      }
      return next;
    });
  }

  private processUpdateMana(action: GameAction): void {
    const player = this.findLoggedInPlayer(action.players);
    if (!player) {
      return;
    }

    if (this.displayMana() === player.mana) {
      return;
    }

    this.displayMana.set(player.mana);
    void this.animateMana(player.mana);
  }

  private processCardPlayed(action: GameAction): void {
    const player = this.findLoggedInPlayer(action.players);
    if (player?.hand) {
      this.displayHand.set(player.hand);
    }
    this.inFlightPlayedCard.set(null);
    this.clearCardPlayState();
  }

  private processNotEnoughMana(action: GameAction): void {
    this.restoreInFlightPlayedCardToHand();
    this.processUpdateMana(action);
    this.triggerManaAttention();
    this.clearCardPlayState();
  }

  private triggerManaAttention(): void {
    this.clearManaAttentionTimeout();
    // Reset to false first, then enable on the next frame so repeated
    // NotEnoughMana events restart the CSS animation reliably.
    this.manaAttentionActive.set(false);
    requestAnimationFrame(() => {
      if (this.isDestroyed) return;
      this.manaAttentionActive.set(true);
      this.manaAttentionTimeout = setTimeout(() => {
        if (this.isDestroyed) return;
        this.manaAttentionActive.set(false);
        this.manaAttentionTimeout = null;
      }, 900);
    });
  }

  private clearManaAttentionTimeout(): void {
    if (this.manaAttentionTimeout !== null) {
      clearTimeout(this.manaAttentionTimeout);
      this.manaAttentionTimeout = null;
    }
  }

  private async processSetInitial(action: GameAction): Promise<void> {
    if (this.gameLoaded) {
      return;
    }
    this.gameOverActive.set(false);
    this.gameOverTextVisible.set(false);
    this.gameOverButtonVisible.set(false);
    this.roomWonActive.set(false);
    this.roomWonTextVisible.set(false);
    if (action.partyMembers) {
      this.displayPartyMembers.set(action.partyMembers);
    }
    if (action.enemies) {
      this.displayEnemies.set(action.enemies);
    }
    if (action.initiative) {
      this.initiativeEntries.set(
        action.initiative.map((entry) => ({
          entry,
          key: initiativeEntryKey(entry),
          animationState: 'active',
        })),
      );
    }
    if (action.board) {
      this.displayBoard.set(action.board);
    }
    const player = action.players?.[0];
    if (player) {
      const hand = player.hand ?? [];
      this.displayHand.set(hand);
      this.displayMana.set(player.mana);
      this.displayedMana.set(player.mana);
    }
    if (action.turn != null) {
      this.displayTurn.set(action.turn);
    }
    this.gameLoaded = true;
  }

  /**
   * Updates the displayed hand to match {@link updatedHand}.
   *
   * Cards that are genuinely new (not already in the hand) are added one by one
   * with a stagger so each slides into the fan separately. Existing cards
   * smoothly reposition to accommodate each new arrival.
   */
  private async animateDrawIntoHand(updatedHand: CardDto[]): Promise<void> {
    const currentHand = this.displayHand();
    const currentIds = new Set(currentHand.map((c) => c.instanceId));
    const newCards = updatedHand.filter((c) => !currentIds.has(c.instanceId));

    if (newCards.length === 0) {
      // No genuinely new cards – just synchronise ordering/state directly.
      this.displayHand.set(updatedHand);
      return;
    }

    // Reorder any already-visible cards to match the server's ordering first,
    // so they slide to make room for the incoming cards.
    const existingInNewOrder = updatedHand.filter((c) => currentIds.has(c.instanceId));
    this.displayHand.set(existingInNewOrder);

    // Add each new card one at a time so the enter animation plays per-card.
    // We only wait CARD_ENTER_STAGGER_MS before adding the next card so the
    // slide-in animations overlap and the whole sequence feels snappy.
    for (const newCard of newCards) {
      if (this.isDestroyed) return;

      // Build the hand slice up to and including this card in the server's order.
      const displayedIds = new Set(this.displayHand().map((c) => c.instanceId));
      const nextHand = updatedHand.filter(
        (c) => displayedIds.has(c.instanceId) || c.instanceId === newCard.instanceId,
      );

      // Mark as entering BEFORE adding to hand so the component is created with isEntering=true.
      this.enteringCardInstanceIds.update((s) => new Set([...s, newCard.instanceId]));
      this.displayHand.set(nextHand);

      // Wait just the stagger interval – the animation itself overlaps with the next card.
      await delay(CARD_ENTER_STAGGER_MS);
      if (this.isDestroyed) return;

      this.enteringCardInstanceIds.update((s) => {
        const next = new Set(s);
        next.delete(newCard.instanceId);
        return next;
      });
    }

    // Ensure the final hand exactly matches the server's order.
    if (!this.isDestroyed) {
      this.displayHand.set(updatedHand);
    }
  }

  private async processDiscardHand(): Promise<void> {
    const currentHand = this.displayHand();
    if (currentHand.length === 0) {
      return;
    }

    const visibleDiscardIds = currentHand.map((card) => card.instanceId);
    this.addCardInstanceIds(this.leavingCardInstanceIds, visibleDiscardIds);
    this.addCardInstanceIds(this.discardHandLeavingCardInstanceIds, visibleDiscardIds);
    this.discardPilePulsing.set(true);
    this.hoveredTooltipCard.set(null);

    await delay(CARD_DISCARD_HAND_FLIGHT_MS);
    if (this.isDestroyed) return;

    this.removeCardInstanceIds(this.leavingCardInstanceIds, visibleDiscardIds);
    this.removeCardInstanceIds(this.discardHandLeavingCardInstanceIds, visibleDiscardIds);

    this.displayHand.set([]);
    this.discardPilePulsing.set(false);
  }

  private addCardInstanceIds(
    cardInstanceIdsSignal: { update: (fn: (existing: Set<string>) => Set<string>) => void },
    ids: string[],
  ): void {
    cardInstanceIdsSignal.update((existing) => new Set([...existing, ...ids]));
  }

  private removeCardInstanceIds(
    cardInstanceIdsSignal: { update: (fn: (existing: Set<string>) => Set<string>) => void },
    ids: string[],
  ): void {
    cardInstanceIdsSignal.update((existing) => {
      const next = new Set(existing);
      for (const id of ids) {
        next.delete(id);
      }
      return next;
    });
  }

  private async processDiscard(action: GameAction): Promise<void> {
    const updatedHand = action.players?.[0]?.hand ?? null;
    const discardedIds = new Set((action.card ?? []).map((card) => card.instanceId));

    if (discardedIds.size === 0) {
      if (updatedHand !== null) {
        this.displayHand.set(updatedHand);
      }
      return;
    }

    const currentHand = this.displayHand();
    const visibleDiscardIds = currentHand
      .filter((card) => discardedIds.has(card.instanceId))
      .map((card) => card.instanceId);

    if (visibleDiscardIds.length === 0) {
      this.displayHand.set(
        updatedHand ?? currentHand.filter((card) => !discardedIds.has(card.instanceId)),
      );
      return;
    }

    this.leavingCardInstanceIds.update((existing) => new Set([...existing, ...visibleDiscardIds]));
    this.discardPilePulsing.set(true);
    this.hoveredTooltipCard.set(null);

    await delay(CARD_DISCARD_FADE_MS);
    if (this.isDestroyed) return;

    this.leavingCardInstanceIds.update((existing) => {
      const next = new Set(existing);
      for (const id of visibleDiscardIds) {
        next.delete(id);
      }
      return next;
    });

    this.displayHand.set(
      updatedHand ?? currentHand.filter((card) => !discardedIds.has(card.instanceId)),
    );
    this.discardPilePulsing.set(false);
  }

  private async processArmor(action: GameAction): Promise<void> {
    await this.processPartyMemberArmor(action, 'physical', (member) => member.armor);
  }

  private async processMagicArmor(action: GameAction): Promise<void> {
    await this.processPartyMemberArmor(action, 'magic', (member) => this.magicArmorValue(member));
  }

  private magicArmorValue(member: PartyMemberDto): number {
    return member.magicArmor ?? 0;
  }

  private async processPartyMemberArmor(
    action: GameAction,
    presentation: ArmorPresentation,
    armorValue: (member: PartyMemberDto) => number,
  ): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    if (updatedMembers.length === 0) {
      return;
    }

    const currentMembers = this.displayPartyMembers();
    const currentById = new Map(currentMembers.map((member) => [member.partyMemberId, member]));
    const mergedMembers = this.mergePartyMembers(updatedMembers);
    const burstsToClear: Array<{ identifier: string; token: number }> = [];

    this.displayPartyMembers.set(mergedMembers);
    this.partyMemberArmorPresentation.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        next[member.identifier] = presentation;
      }
      return next;
    });

    this.partyMemberArmorBursts.update((current) => {
      const next = { ...current };

      for (const member of updatedMembers) {
        const previous = currentById.get(member.partyMemberId);
        const currentArmor = armorValue(member);
        const previousArmor = previous ? armorValue(previous) : currentArmor;
        const delta = currentArmor - previousArmor;
        if (delta === 0) {
          continue;
        }

        const token = ++this.armorBurstToken;
        next[member.identifier] = { delta, token };
        burstsToClear.push({ identifier: member.identifier, token });
      }

      return next;
    });

    if (burstsToClear.length === 0) {
      return;
    }

    await delay(ARMOR_BURST_MS);
    if (this.isDestroyed) return;

    this.partyMemberArmorBursts.update((current) => {
      const next = { ...current };
      for (const burst of burstsToClear) {
        if (next[burst.identifier]?.token === burst.token) {
          delete next[burst.identifier];
        }
      }
      return next;
    });
  }

  private mergePartyMembers(updatedMembers: PartyMemberDto[]): PartyMemberDto[] {
    const currentMembers = this.displayPartyMembers();
    if (currentMembers.length === 0) {
      return updatedMembers;
    }

    const updatesById = new Map(updatedMembers.map((member) => [member.partyMemberId, member]));
    const currentIds = new Set(currentMembers.map((member) => member.partyMemberId));

    return [
      ...currentMembers.map((member) => updatesById.get(member.partyMemberId) ?? member),
      ...updatedMembers.filter((member) => !currentIds.has(member.partyMemberId)),
    ];
  }

  private async processStrength(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    const updatedEnemies = action.enemies ?? [];
    if (updatedMembers.length === 0 && updatedEnemies.length === 0) {
      return;
    }

    const memberBurstsToClear: Array<{ identifier: string; token: number }> = [];
    const enemyBurstsToClear: Array<{ instanceId: string; token: number }> = [];

    if (updatedMembers.length > 0) {
      const currentById = new Map(this.displayPartyMembers().map((m) => [m.partyMemberId, m]));
      this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

      this.partyMemberStrengthBursts.update((current) => {
        const next = { ...current };
        for (const member of updatedMembers) {
          const previous = currentById.get(member.partyMemberId);
          const currentStrength = Number.isFinite(member.strength) ? member.strength : 0;
          const previousStrength =
            previous && Number.isFinite(previous.strength) ? previous.strength : currentStrength;
          const delta = currentStrength - previousStrength;
          if (delta === 0) {
            continue;
          }
          const token = ++this.strengthBurstToken;
          next[member.identifier] = { delta, token };
          memberBurstsToClear.push({ identifier: member.identifier, token });
        }
        return next;
      });
    }

    if (updatedEnemies.length > 0) {
      const currentByInstanceId = new Map(this.displayEnemies().map((e) => [e.instanceId, e]));
      this.displayEnemies.set(this.mergeEnemies(updatedEnemies));

      this.enemyStrengthBursts.update((current) => {
        const next = { ...current };
        for (const enemy of updatedEnemies) {
          const previous = currentByInstanceId.get(enemy.instanceId);
          const currentStrength = Number.isFinite(enemy.strength) ? enemy.strength : 0;
          const previousStrength =
            previous && Number.isFinite(previous.strength) ? previous.strength : currentStrength;
          const delta = currentStrength - previousStrength;
          if (delta === 0) {
            continue;
          }
          const token = ++this.strengthBurstToken;
          next[enemy.instanceId] = { delta, token };
          enemyBurstsToClear.push({ instanceId: enemy.instanceId, token });
        }
        return next;
      });
    }

    if (memberBurstsToClear.length === 0 && enemyBurstsToClear.length === 0) {
      return;
    }

    await delay(STRENGTH_BURST_MS);
    if (this.isDestroyed) return;

    this.partyMemberStrengthBursts.update((current) => {
      const next = { ...current };
      for (const burst of memberBurstsToClear) {
        if (next[burst.identifier]?.token === burst.token) {
          delete next[burst.identifier];
        }
      }
      return next;
    });

    this.enemyStrengthBursts.update((current) => {
      const next = { ...current };
      for (const burst of enemyBurstsToClear) {
        if (next[burst.instanceId]?.token === burst.token) {
          delete next[burst.instanceId];
        }
      }
      return next;
    });
  }

  private async processDefense(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    const updatedEnemies = action.enemies ?? [];
    if (updatedMembers.length === 0 && updatedEnemies.length === 0) {
      return;
    }

    const memberBurstsToClear: Array<{ identifier: string; token: number }> = [];
    const enemyBurstsToClear: Array<{ instanceId: string; token: number }> = [];

    if (updatedMembers.length > 0) {
      const currentById = new Map(this.displayPartyMembers().map((m) => [m.partyMemberId, m]));
      this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

      this.partyMemberDefenseBursts.update((current) => {
        const next = { ...current };
        for (const member of updatedMembers) {
          const previous = currentById.get(member.partyMemberId);
          const currentDefense = Number.isFinite(member.defense) ? member.defense : 0;
          const previousDefense =
            previous && Number.isFinite(previous.defense) ? previous.defense : currentDefense;
          const delta = currentDefense - previousDefense;
          if (delta === 0) {
            continue;
          }
          const token = ++this.defenseBurstToken;
          next[member.identifier] = { delta, token };
          memberBurstsToClear.push({ identifier: member.identifier, token });
        }
        return next;
      });
    }

    if (updatedEnemies.length > 0) {
      const currentByInstanceId = new Map(this.displayEnemies().map((e) => [e.instanceId, e]));
      this.displayEnemies.set(this.mergeEnemies(updatedEnemies));

      this.enemyDefenseBursts.update((current) => {
        const next = { ...current };
        for (const enemy of updatedEnemies) {
          const previous = currentByInstanceId.get(enemy.instanceId);
          const currentDefense = Number.isFinite(enemy.defense) ? enemy.defense : 0;
          const previousDefense =
            previous && Number.isFinite(previous.defense) ? previous.defense : currentDefense;
          const delta = currentDefense - previousDefense;
          if (delta === 0) {
            continue;
          }
          const token = ++this.defenseBurstToken;
          next[enemy.instanceId] = { delta, token };
          enemyBurstsToClear.push({ instanceId: enemy.instanceId, token });
        }
        return next;
      });
    }

    if (memberBurstsToClear.length === 0 && enemyBurstsToClear.length === 0) {
      return;
    }

    await delay(DEFENSE_BURST_MS);
    if (this.isDestroyed) return;

    this.partyMemberDefenseBursts.update((current) => {
      const next = { ...current };
      for (const burst of memberBurstsToClear) {
        if (next[burst.identifier]?.token === burst.token) {
          delete next[burst.identifier];
        }
      }
      return next;
    });

    this.enemyDefenseBursts.update((current) => {
      const next = { ...current };
      for (const burst of enemyBurstsToClear) {
        if (next[burst.instanceId]?.token === burst.token) {
          delete next[burst.instanceId];
        }
      }
      return next;
    });
  }

  private async processSpeed(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    const updatedEnemies = action.enemies ?? [];
    if (updatedMembers.length === 0 && updatedEnemies.length === 0) {
      return;
    }

    const memberBurstsToClear: Array<{ identifier: string; token: number }> = [];
    const enemyBurstsToClear: Array<{ instanceId: string; token: number }> = [];

    if (updatedMembers.length > 0) {
      const currentById = new Map(this.displayPartyMembers().map((m) => [m.partyMemberId, m]));
      this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

      this.partyMemberSpeedBursts.update((current) => {
        const next = { ...current };
        for (const member of updatedMembers) {
          const previous = currentById.get(member.partyMemberId);
          const currentSpeed = Number.isFinite(member.speed) ? member.speed : 0;
          const previousSpeed =
            previous && Number.isFinite(previous.speed) ? previous.speed : currentSpeed;
          const delta = currentSpeed - previousSpeed;
          if (delta === 0) {
            continue;
          }
          const token = ++this.speedBurstToken;
          next[member.identifier] = { delta, token };
          memberBurstsToClear.push({ identifier: member.identifier, token });
        }
        return next;
      });
    }

    if (updatedEnemies.length > 0) {
      const currentByInstanceId = new Map(this.displayEnemies().map((e) => [e.instanceId, e]));
      this.displayEnemies.set(this.mergeEnemies(updatedEnemies));

      this.enemySpeedBursts.update((current) => {
        const next = { ...current };
        for (const enemy of updatedEnemies) {
          const previous = currentByInstanceId.get(enemy.instanceId);
          const currentSpeed = Number.isFinite(enemy.speed) ? enemy.speed : 0;
          const previousSpeed =
            previous && Number.isFinite(previous.speed) ? previous.speed : currentSpeed;
          const delta = currentSpeed - previousSpeed;
          if (delta === 0) {
            continue;
          }
          const token = ++this.speedBurstToken;
          next[enemy.instanceId] = { delta, token };
          enemyBurstsToClear.push({ instanceId: enemy.instanceId, token });
        }
        return next;
      });
    }

    if (memberBurstsToClear.length === 0 && enemyBurstsToClear.length === 0) {
      return;
    }

    await delay(STRENGTH_BURST_MS);
    if (this.isDestroyed) return;

    this.partyMemberSpeedBursts.update((current) => {
      const next = { ...current };
      for (const burst of memberBurstsToClear) {
        if (next[burst.identifier]?.token === burst.token) {
          delete next[burst.identifier];
        }
      }
      return next;
    });

    this.enemySpeedBursts.update((current) => {
      const next = { ...current };
      for (const burst of enemyBurstsToClear) {
        if (next[burst.instanceId]?.token === burst.token) {
          delete next[burst.instanceId];
        }
      }
      return next;
    });
  }

  private async processAttack(action: GameAction): Promise<void> {
    const attackingMembers = action.partyMembers ?? [];

    if (attackingMembers.length === 0) {
      return;
    }

    const clearParty: Array<{ identifier: string; token: number }> = [];

    this.partyMemberAttackState.update((current) => {
      const next = { ...current };
      for (const member of attackingMembers) {
        const token = ++this.attackAnimationToken;
        next[member.identifier] = { token };
        clearParty.push({ identifier: member.identifier, token });
      }
      return next;
    });

    await delay(ATTACK_ANIMATION_MS);
    if (this.isDestroyed) return;

    this.partyMemberAttackState.update((current) => {
      const next = { ...current };
      for (const item of clearParty) {
        if (next[item.identifier]?.token === item.token) {
          delete next[item.identifier];
        }
      }
      return next;
    });
  }

  private async processEnemyAttack(action: GameAction): Promise<void> {
    const attackingEnemies = action.enemies ?? [];

    if (attackingEnemies.length === 0) {
      return;
    }

    const clearEnemies: Array<{ instanceId: string; token: number }> = [];

    this.enemyAttackState.update((current) => {
      const next = { ...current };
      for (const enemy of attackingEnemies) {
        const token = ++this.attackAnimationToken;
        next[enemy.instanceId] = { token };
        clearEnemies.push({ instanceId: enemy.instanceId, token });
      }
      return next;
    });

    await delay(ATTACK_ANIMATION_MS);
    if (this.isDestroyed) return;

    this.enemyAttackState.update((current) => {
      const next = { ...current };
      for (const item of clearEnemies) {
        if (next[item.instanceId]?.token === item.token) {
          delete next[item.instanceId];
        }
      }
      return next;
    });
  }

  private async processDamage(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    const updatedEnemies = action.enemies ?? [];

    if (updatedMembers.length === 0 && updatedEnemies.length === 0) {
      return;
    }

    const currentMembersByIdentifier = new Map(
      this.displayPartyMembers().map((m) => [m.identifier, m]),
    );
    const currentEnemiesByInstanceId = new Map(this.displayEnemies().map((e) => [e.instanceId, e]));

    // Update HP immediately so health bars start transitioning.
    if (updatedMembers.length > 0) {
      this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));
    }
    if (updatedEnemies.length > 0) {
      this.displayEnemies.set(this.mergeEnemies(updatedEnemies));
    }

    const clearParty: Array<{ identifier: string; token: number }> = [];
    const clearEnemies: Array<{ instanceId: string; token: number }> = [];

    if (updatedMembers.length > 0) {
      this.partyMemberDefendState.update((current) => {
        const next = { ...current };
        for (const member of updatedMembers) {
          const previous = currentMembersByIdentifier.get(member.identifier);
          const delta = previous != null ? member.health - previous.health : 0;
          const token = ++this.defendAnimationToken;
          next[member.identifier] = { delta, token };
          clearParty.push({ identifier: member.identifier, token });
        }
        return next;
      });
    }

    if (updatedEnemies.length > 0) {
      this.enemyDefendState.update((current) => {
        const next = { ...current };
        for (const enemy of updatedEnemies) {
          const previous = currentEnemiesByInstanceId.get(enemy.instanceId);
          const delta = previous != null ? enemy.hitPoints - previous.hitPoints : 0;
          const token = ++this.defendAnimationToken;
          next[enemy.instanceId] = { delta, token };
          clearEnemies.push({ instanceId: enemy.instanceId, token });
        }
        return next;
      });
    }

    const hasBursts = clearParty.length > 0 || clearEnemies.length > 0;
    if (!hasBursts) return;

    await delay(DAMAGE_ANIMATION_MS);
    if (this.isDestroyed) return;

    this.partyMemberDefendState.update((current) => {
      const next = { ...current };
      for (const item of clearParty) {
        if (next[item.identifier]?.token === item.token) {
          delete next[item.identifier];
        }
      }
      return next;
    });

    this.enemyDefendState.update((current) => {
      const next = { ...current };
      for (const item of clearEnemies) {
        if (next[item.instanceId]?.token === item.token) {
          delete next[item.instanceId];
        }
      }
      return next;
    });
  }

  private processEnemyDied(action: GameAction): void {
    const updatedEnemies = action.enemies ?? [];
    if (updatedEnemies.length === 0) {
      return;
    }

    this.displayEnemies.set(this.mergeEnemies(updatedEnemies));

    this.enemyDefendState.update((current) => {
      const next = { ...current };
      for (const enemy of updatedEnemies) {
        if (this.isEnemyDead(enemy)) {
          delete next[enemy.instanceId];
        }
      }
      return next;
    });
  }

  private async processEnemyArmor(action: GameAction): Promise<void> {
    const updatedEnemies = action.enemies ?? [];
    if (updatedEnemies.length === 0) {
      return;
    }

    const currentByInstanceId = new Map(
      this.displayEnemies().map((enemy) => [enemy.instanceId, enemy]),
    );
    const mergedEnemies = this.mergeEnemies(updatedEnemies);
    const burstsToClear: Array<{ instanceId: string; token: number }> = [];

    this.displayEnemies.set(mergedEnemies);

    this.enemyArmorBursts.update((current) => {
      const next = { ...current };

      for (const enemy of updatedEnemies) {
        const previous = currentByInstanceId.get(enemy.instanceId);
        const delta = previous ? enemy.armor - previous.armor : 0;
        if (delta === 0) {
          continue;
        }

        const token = ++this.armorBurstToken;
        next[enemy.instanceId] = { delta, token };
        burstsToClear.push({ instanceId: enemy.instanceId, token });
      }

      return next;
    });

    if (burstsToClear.length === 0) {
      return;
    }

    await delay(ARMOR_BURST_MS);
    if (this.isDestroyed) return;

    this.enemyArmorBursts.update((current) => {
      const next = { ...current };
      for (const burst of burstsToClear) {
        if (next[burst.instanceId]?.token === burst.token) {
          delete next[burst.instanceId];
        }
      }
      return next;
    });
  }

  private async processEnemyHealing(action: GameAction): Promise<void> {
    const updatedEnemies = action.enemies ?? [];
    if (updatedEnemies.length === 0) {
      return;
    }

    const currentByInstanceId = new Map(
      this.displayEnemies().map((enemy) => [enemy.instanceId, enemy]),
    );
    this.displayEnemies.set(this.mergeEnemies(updatedEnemies));

    const clearEnemies: Array<{ instanceId: string; token: number }> = [];

    this.enemyHealState.update((current) => {
      const next = { ...current };
      for (const enemy of updatedEnemies) {
        const previous = currentByInstanceId.get(enemy.instanceId);
        const delta = previous != null ? enemy.hitPoints - previous.hitPoints : 0;
        if (delta === 0) {
          continue;
        }
        const token = ++this.resourceGainAnimationToken;
        next[enemy.instanceId] = { delta, token };
        clearEnemies.push({ instanceId: enemy.instanceId, token });
      }
      return next;
    });

    if (clearEnemies.length === 0) {
      return;
    }

    await delay(HEAL_ANIMATION_MS);
    if (this.isDestroyed) return;

    this.enemyHealState.update((current) => {
      const next = { ...current };
      for (const item of clearEnemies) {
        if (next[item.instanceId]?.token === item.token) {
          delete next[item.instanceId];
        }
      }
      return next;
    });
  }

  private async processHealing(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    if (updatedMembers.length === 0) {
      return;
    }

    const currentByIdentifier = new Map(
      this.displayPartyMembers().map((member) => [member.identifier, member]),
    );
    this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

    const clearMembers: Array<{ identifier: string; token: number }> = [];

    this.partyMemberHealState.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        const previous = currentByIdentifier.get(member.identifier);
        const delta = previous != null ? member.health - previous.health : 0;
        if (delta === 0) {
          continue;
        }
        const token = ++this.resourceGainAnimationToken;
        next[member.identifier] = { delta, token };
        clearMembers.push({ identifier: member.identifier, token });
      }
      return next;
    });

    if (clearMembers.length === 0) {
      return;
    }

    await delay(HEAL_ANIMATION_MS);
    if (this.isDestroyed) return;

    this.partyMemberHealState.update((current) => {
      const next = { ...current };
      for (const item of clearMembers) {
        if (next[item.identifier]?.token === item.token) {
          delete next[item.identifier];
        }
      }
      return next;
    });
  }

  private async processMana(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    if (updatedMembers.length === 0) {
      return;
    }

    this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

    const clearMembers: Array<{ identifier: string; token: number }> = [];

    this.partyMemberManaState.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        const token = ++this.resourceGainAnimationToken;
        next[member.identifier] = { delta: null, token };
        clearMembers.push({ identifier: member.identifier, token });
      }
      return next;
    });

    await delay(MANA_ANIMATION_MS);
    if (this.isDestroyed) return;

    this.partyMemberManaState.update((current) => {
      const next = { ...current };
      for (const item of clearMembers) {
        if (next[item.identifier]?.token === item.token) {
          delete next[item.identifier];
        }
      }
      return next;
    });
  }

  private processPartyMemberDied(action: GameAction): void {
    const updatedMembers = action.partyMembers ?? [];
    if (updatedMembers.length === 0) {
      return;
    }

    this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

    this.partyMemberArmorBursts.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        if (this.isPartyMemberDead(member)) {
          delete next[member.identifier];
        }
      }
      return next;
    });

    this.partyMemberAttackState.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        if (this.isPartyMemberDead(member)) {
          delete next[member.identifier];
        }
      }
      return next;
    });

    this.partyMemberDefendState.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        if (this.isPartyMemberDead(member)) {
          delete next[member.identifier];
        }
      }
      return next;
    });

    this.partyMemberManaState.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        if (this.isPartyMemberDead(member)) {
          delete next[member.identifier];
        }
      }
      return next;
    });

    this.partyMemberHealState.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        if (this.isPartyMemberDead(member)) {
          delete next[member.identifier];
        }
      }
      return next;
    });

    this.partyMemberArmorPresentation.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        if (this.isPartyMemberDead(member)) {
          delete next[member.identifier];
        }
      }
      return next;
    });
  }

  private async processGameOver(action: GameAction): Promise<void> {
    if (action.partyMembers?.length) {
      const mergedMembers = this.mergePartyMembers(action.partyMembers);
      this.displayPartyMembers.set(mergedMembers);
    }

    if (action.enemies?.length) {
      const mergedEnemies = this.mergeEnemies(action.enemies);
      this.displayEnemies.set(mergedEnemies);
    }

    if (action.initiative) {
      this.initiativeEntries.set(
        action.initiative.map((entry) => ({
          entry,
          key: initiativeEntryKey(entry),
          animationState: 'active',
        })),
      );
    }

    this.activePile.set(null);
    this.isAnyCardDragging.set(false);
    this.drawPilePulsing.set(false);
    this.discardPilePulsing.set(false);
    this.clearCardPlayState();

    if (this.gameOverActive()) {
      return;
    }

    this.roomWonActive.set(false);
    this.roomWonTextVisible.set(false);
    this.gameOverTextVisible.set(false);
    this.gameOverButtonVisible.set(false);
    this.gameOverActive.set(true);

    await delay(GAME_OVER_TEXT_DELAY_MS);
    if (this.isDestroyed) return;

    this.gameOverTextVisible.set(true);

    await delay(Math.max(GAME_OVER_FINAL_DELAY_MS, 0));
    if (this.isDestroyed) return;

    await delay(GAME_OVER_BUTTON_DELAY_MS);
    if (this.isDestroyed) return;

    this.gameOverButtonVisible.set(true);
  }

  private async processMomentumAdd(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    if (updatedMembers.length === 0) {
      return;
    }

    this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));
  }

  private async processDaggers(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    const updatedEnemies = action.enemies ?? [];

    if (updatedMembers.length === 0 && updatedEnemies.length === 0) {
      return;
    }

    let hasFadeOut = false;

    if (updatedMembers.length > 0) {
      const currentById = new Map(this.displayPartyMembers().map((m) => [m.identifier, m]));
      this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

      this.partyMemberDaggersAnimationState.update((current) => {
        const next = { ...current };
        for (const member of updatedMembers) {
          const previous = currentById.get(member.identifier);
          const oldDaggers = previous?.daggers ?? 0;
          const newDaggers = member.daggers ?? 0;
          if (oldDaggers !== newDaggers) {
            next[member.identifier] = ++this.resourceGainAnimationToken;
            if (newDaggers === 0) {
              hasFadeOut = true;
            }
          }
        }
        return next;
      });
    }

    if (updatedEnemies.length > 0) {
      const currentById = new Map(this.displayEnemies().map((e) => [e.instanceId, e]));
      this.displayEnemies.set(this.mergeEnemies(updatedEnemies));

      this.enemyDaggersAnimationState.update((current) => {
        const next = { ...current };
        for (const enemy of updatedEnemies) {
          const previous = currentById.get(enemy.instanceId);
          const oldDaggers = previous?.daggers ?? 0;
          const newDaggers = enemy.daggers ?? 0;
          if (oldDaggers !== newDaggers) {
            next[enemy.instanceId] = ++this.resourceGainAnimationToken;
            if (newDaggers === 0) {
              hasFadeOut = true;
            }
          }
        }
        return next;
      });
    }

    if (hasFadeOut) {
      await delay(600);
    }

    // Clean up animation state to avoid infinite memory growth
    // when new dagger tokens are continually created across many turns.
    if (this.isDestroyed) return;
    this.partyMemberDaggersAnimationState.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        delete next[member.identifier];
      }
      return next;
    });
    this.enemyDaggersAnimationState.update((current) => {
      const next = { ...current };
      for (const enemy of updatedEnemies) {
        delete next[enemy.instanceId];
      }
      return next;
    });
  }

  private async processSpirit(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];

    if (updatedMembers.length === 0) {
      return;
    }

    let hasFadeOut = false;
    let hasGain = false;

    const currentById = new Map(this.displayPartyMembers().map((m) => [m.identifier, m]));
    this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

    this.partyMemberSpiritAnimationState.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        const previous = currentById.get(member.identifier);
        const oldSpirit = previous?.spirit ?? 0;
        const newSpirit = member.spirit ?? 0;
        if (oldSpirit !== newSpirit) {
          next[member.identifier] = ++this.resourceGainAnimationToken;
          if (newSpirit === 0) {
            hasFadeOut = true;
          } else {
            hasGain = true;
          }
        }
      }
      return next;
    });

    if (hasGain || hasFadeOut) {
      await delay(600);
    }

    if (this.isDestroyed) return;
    this.partyMemberSpiritAnimationState.update((current) => {
      const next = { ...current };
      for (const member of updatedMembers) {
        delete next[member.identifier];
      }
      return next;
    });
  }

  private async processPoison(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    const updatedEnemies = action.enemies ?? [];

    if (updatedMembers.length === 0 && updatedEnemies.length === 0) {
      return;
    }

    const clearPoisonMembers: Array<{ identifier: string; token: number }> = [];
    const clearPoisonEnemies: Array<{ instanceId: string; token: number }> = [];
    const clearDefendParty: Array<{ identifier: string; token: number }> = [];
    const clearDefendEnemies: Array<{ instanceId: string; token: number }> = [];

    if (updatedMembers.length > 0) {
      const currentByPartyMemberId = new Map(
        this.displayPartyMembers().map((m) => [m.partyMemberId, m]),
      );
      this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

      this.partyMemberPoisonState.update((current) => {
        const next = { ...current };
        for (const member of updatedMembers) {
          const previous = currentByPartyMemberId.get(member.partyMemberId);
          const delta = previous != null ? (member.poison ?? 0) - (previous.poison ?? 0) : 0;
          if (delta === 0) {
            continue;
          }
          const token = ++this.resourceGainAnimationToken;
          next[member.identifier] = { delta, token };
          clearPoisonMembers.push({ identifier: member.identifier, token });
        }
        return next;
      });

      this.partyMemberDefendState.update((current) => {
        const next = { ...current };
        for (const member of updatedMembers) {
          const previous = currentByPartyMemberId.get(member.partyMemberId);
          const delta = previous != null ? (member.poison ?? 0) - (previous.poison ?? 0) : 0;
          if (delta === 0) continue;
          const token = ++this.defendAnimationToken;
          next[member.identifier] = { delta: 0, token };
          clearDefendParty.push({ identifier: member.identifier, token });
        }
        return next;
      });
    }

    if (updatedEnemies.length > 0) {
      const currentByEnemyInstanceId = new Map(this.displayEnemies().map((e) => [e.instanceId, e]));
      this.displayEnemies.set(this.mergeEnemies(updatedEnemies));

      this.enemyPoisonState.update((current) => {
        const next = { ...current };
        for (const enemy of updatedEnemies) {
          const previous = currentByEnemyInstanceId.get(enemy.instanceId);
          const delta = previous != null ? (enemy.poison ?? 0) - (previous.poison ?? 0) : 0;
          if (delta === 0) {
            continue;
          }
          const token = ++this.resourceGainAnimationToken;
          next[enemy.instanceId] = { delta, token };
          clearPoisonEnemies.push({ instanceId: enemy.instanceId, token });
        }
        return next;
      });

      this.enemyDefendState.update((current) => {
        const next = { ...current };
        for (const enemy of updatedEnemies) {
          const previous = currentByEnemyInstanceId.get(enemy.instanceId);
          const delta = previous != null ? (enemy.poison ?? 0) - (previous.poison ?? 0) : 0;
          if (delta === 0) continue;
          const token = ++this.defendAnimationToken;
          next[enemy.instanceId] = { delta: 0, token };
          clearDefendEnemies.push({ instanceId: enemy.instanceId, token });
        }
        return next;
      });
    }

    const hasDefends = clearDefendParty.length > 0 || clearDefendEnemies.length > 0;
    const hasPoisons = clearPoisonMembers.length > 0 || clearPoisonEnemies.length > 0;

    if (!hasDefends && !hasPoisons) return;

    await Promise.all([
      hasDefends ? delay(DAMAGE_ANIMATION_MS) : Promise.resolve(),
      hasPoisons ? delay(POISON_ANIMATION_MS) : Promise.resolve(),
    ]);
    if (this.isDestroyed) return;

    if (hasDefends) {
      this.partyMemberDefendState.update((current) => {
        const next = { ...current };
        for (const item of clearDefendParty) {
          if (next[item.identifier]?.token === item.token) {
            delete next[item.identifier];
          }
        }
        return next;
      });

      this.enemyDefendState.update((current) => {
        const next = { ...current };
        for (const item of clearDefendEnemies) {
          if (next[item.instanceId]?.token === item.token) {
            delete next[item.instanceId];
          }
        }
        return next;
      });
    }

    if (hasPoisons) {
      this.partyMemberPoisonState.update((current) => {
        const next = { ...current };
        for (const item of clearPoisonMembers) {
          if (next[item.identifier]?.token === item.token) {
            delete next[item.identifier];
          }
        }
        return next;
      });

      this.enemyPoisonState.update((current) => {
        const next = { ...current };
        for (const item of clearPoisonEnemies) {
          if (next[item.instanceId]?.token === item.token) {
            delete next[item.instanceId];
          }
        }
        return next;
      });
    }
  }

  private async processPoisonConsumed(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    const updatedEnemies = action.enemies ?? [];

    if (updatedMembers.length === 0 && updatedEnemies.length === 0) {
      return;
    }

    const currentMembersByIdentifier = new Map(
      this.displayPartyMembers().map((m) => [m.identifier, m]),
    );
    const currentEnemiesByInstanceId = new Map(this.displayEnemies().map((e) => [e.instanceId, e]));

    // Update state immediately so health bars and poison values start transitioning.
    if (updatedMembers.length > 0) {
      this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));
    }
    if (updatedEnemies.length > 0) {
      this.displayEnemies.set(this.mergeEnemies(updatedEnemies));
    }

    const clearDefendParty: Array<{ identifier: string; token: number }> = [];
    const clearDefendEnemies: Array<{ instanceId: string; token: number }> = [];
    const clearPoisonMembers: Array<{ identifier: string; token: number }> = [];
    const clearPoisonEnemies: Array<{ instanceId: string; token: number }> = [];

    // Process members
    if (updatedMembers.length > 0) {
      // Defend state
      this.partyMemberDefendState.update((current) => {
        const next = { ...current };
        for (const member of updatedMembers) {
          const previous = currentMembersByIdentifier.get(member.identifier);
          const delta = previous != null ? member.health - previous.health : 0;
          if (delta === 0) continue;
          const token = ++this.defendAnimationToken;
          next[member.identifier] = { delta, token };
          clearDefendParty.push({ identifier: member.identifier, token });
        }
        return next;
      });

      // Poison state
      this.partyMemberPoisonState.update((current) => {
        const next = { ...current };
        for (const member of updatedMembers) {
          const previous = currentMembersByIdentifier.get(member.identifier);
          const delta = previous != null ? (member.poison ?? 0) - (previous.poison ?? 0) : 0;
          if (delta === 0) continue;
          const token = ++this.resourceGainAnimationToken;
          next[member.identifier] = { delta: 0, token };
          clearPoisonMembers.push({ identifier: member.identifier, token });
        }
        return next;
      });
    }

    // Process enemies
    if (updatedEnemies.length > 0) {
      // Defend state
      this.enemyDefendState.update((current) => {
        const next = { ...current };
        for (const enemy of updatedEnemies) {
          const previous = currentEnemiesByInstanceId.get(enemy.instanceId);
          const delta = previous != null ? enemy.hitPoints - previous.hitPoints : 0;
          if (delta === 0) continue;
          const token = ++this.defendAnimationToken;
          next[enemy.instanceId] = { delta, token };
          clearDefendEnemies.push({ instanceId: enemy.instanceId, token });
        }
        return next;
      });

      // Poison state
      this.enemyPoisonState.update((current) => {
        const next = { ...current };
        for (const enemy of updatedEnemies) {
          const previous = currentEnemiesByInstanceId.get(enemy.instanceId);
          const delta = previous != null ? (enemy.poison ?? 0) - (previous.poison ?? 0) : 0;
          if (delta === 0) continue;
          const token = ++this.resourceGainAnimationToken;
          next[enemy.instanceId] = { delta: 0, token };
          clearPoisonEnemies.push({ instanceId: enemy.instanceId, token });
        }
        return next;
      });
    }

    const hasDefends = clearDefendParty.length > 0 || clearDefendEnemies.length > 0;
    const hasPoisons = clearPoisonMembers.length > 0 || clearPoisonEnemies.length > 0;

    if (!hasDefends && !hasPoisons) return;

    await Promise.all([
      hasDefends ? delay(DAMAGE_ANIMATION_MS) : Promise.resolve(),
      hasPoisons ? delay(POISON_ANIMATION_MS) : Promise.resolve(),
    ]);
    if (this.isDestroyed) return;

    if (hasDefends) {
      this.partyMemberDefendState.update((current) => {
        const next = { ...current };
        for (const item of clearDefendParty) {
          if (next[item.identifier]?.token === item.token) {
            delete next[item.identifier];
          }
        }
        return next;
      });

      this.enemyDefendState.update((current) => {
        const next = { ...current };
        for (const item of clearDefendEnemies) {
          if (next[item.instanceId]?.token === item.token) {
            delete next[item.instanceId];
          }
        }
        return next;
      });
    }

    if (hasPoisons) {
      this.partyMemberPoisonState.update((current) => {
        const next = { ...current };
        for (const item of clearPoisonMembers) {
          if (next[item.identifier]?.token === item.token) {
            delete next[item.identifier];
          }
        }
        return next;
      });

      this.enemyPoisonState.update((current) => {
        const next = { ...current };
        for (const item of clearPoisonEnemies) {
          if (next[item.instanceId]?.token === item.token) {
            delete next[item.instanceId];
          }
        }
        return next;
      });
    }
  }

  private async processMomentumRemove(action: GameAction): Promise<void> {
    const updatedMembers = action.partyMembers ?? [];
    if (updatedMembers.length === 0) {
      return;
    }

    let maxRemoved = 0;
    const currentById = new Map(this.displayPartyMembers().map((m) => [m.partyMemberId, m]));

    for (const member of updatedMembers) {
      const current = currentById.get(member.partyMemberId)?.momentum ?? 0;
      const next = member.momentum ?? 0;
      if (current > next) {
        maxRemoved = Math.max(maxRemoved, current - next);
      }
    }

    this.displayPartyMembers.set(this.mergePartyMembers(updatedMembers));

    if (maxRemoved > 0) {
      const totalTime = maxRemoved * MOMENTUM_REMOVE_STAGGER_MS + MOMENTUM_REMOVE_FADE_MS;
      await delay(totalTime);
    }
  }

  private async processRoomWon(action: GameAction): Promise<void> {
    if (action.partyMembers?.length) {
      const mergedMembers = this.mergePartyMembers(action.partyMembers);
      this.displayPartyMembers.set(mergedMembers);
    }

    if (action.enemies?.length) {
      const mergedEnemies = this.mergeEnemies(action.enemies);
      this.displayEnemies.set(mergedEnemies);
    }

    if (action.initiative) {
      this.initiativeEntries.set(
        action.initiative.map((entry) => ({
          entry,
          key: initiativeEntryKey(entry),
          animationState: 'active',
        })),
      );
    }

    this.activePile.set(null);
    this.isAnyCardDragging.set(false);
    this.drawPilePulsing.set(false);
    this.discardPilePulsing.set(false);
    this.clearCardPlayState();

    if (this.gameOverActive() || this.roomWonActive()) {
      return;
    }

    this.gameOverActive.set(false);
    this.gameOverTextVisible.set(false);
    this.gameOverButtonVisible.set(false);
    this.roomWonTextVisible.set(false);
    this.roomWonActive.set(true);

    await delay(ROOM_WON_TEXT_DELAY_MS);
    if (this.isDestroyed) return;

    this.roomWonTextVisible.set(true);

    await delay(ROOM_WON_FINAL_DELAY_MS);
    if (this.isDestroyed) return;

    await delay(ROOM_WON_TRANSITION_DELAY_MS);
    if (this.isDestroyed) return;

    this.roomWon.emit();
  }

  protected enemyIntentions(enemy: EnemyDto): NonNullable<EnemyDto['intentions']> {
    if (this.isEnemyDead(enemy)) {
      return [];
    }
    return enemy.intentions ?? [];
  }

  private isEnemyDead(enemy: EnemyDto): boolean {
    return enemy.hitPoints <= 0;
  }

  private isPartyMemberDead(member: PartyMemberDto): boolean {
    return member.health <= 0;
  }

  private mergeEnemies(updatedEnemies: EnemyDto[]): EnemyDto[] {
    const currentEnemies = this.displayEnemies();
    if (currentEnemies.length === 0) {
      return updatedEnemies;
    }

    const updatesById = new Map(updatedEnemies.map((e) => [e.instanceId, e]));
    const currentIds = new Set(currentEnemies.map((e) => e.instanceId));

    return [
      ...currentEnemies.map((e) => updatesById.get(e.instanceId) ?? e),
      ...updatedEnemies.filter((e) => !currentIds.has(e.instanceId)),
    ];
  }

  private async processUpdateTurn(action: GameAction): Promise<void> {
    if (action.enemies) {
      this.displayEnemies.set(this.mergeEnemies(action.enemies));
    }

    const newTurn = action.turn ?? 0;
    // Kick off the grow + wobble animation with the old number still showing.
    this.turnAnimating.set(true);
    // Swap to the new number at the animation peak (~45 % through 800 ms).
    await delay(TURN_UPDATE_PEAK_MS);
    if (this.isDestroyed) return;
    this.displayTurn.set(newTurn);
    // Let the shrink-back + tail wobble finish.
    await delay(TURN_UPDATE_TAIL_MS);
    if (this.isDestroyed) return;
    this.turnAnimating.set(false);
  }

  private async processUpdateInitiative(action: GameAction): Promise<void> {
    if (action.enemies) {
      this.displayEnemies.set(this.mergeEnemies(action.enemies));
    }

    const newEntries = action.initiative ?? [];
    const newKeySet = new Set(newEntries.map(initiativeEntryKey));
    const currentEntries = this.initiativeEntries();
    const currentKeySet = new Set(currentEntries.map((e) => e.key));

    // ── 1. Animate out entries that are no longer present ──────────────────
    const leavingKeys = new Set(
      currentEntries.filter((e) => !newKeySet.has(e.key)).map((e) => e.key),
    );

    if (leavingKeys.size > 0) {
      this.initiativeEntries.update((entries) =>
        entries.map((e) => (leavingKeys.has(e.key) ? { ...e, animationState: 'leaving' } : e)),
      );
      await delay(LEAVE_DURATION_MS);
      if (this.isDestroyed) return;
      this.initiativeEntries.update((entries) => entries.filter((e) => !leavingKeys.has(e.key)));
    }

    // ── 2. Reorder the surviving entries to match the new initiative order ──
    const survivorsInNewOrder: InitiativeEntryState[] = newEntries
      .filter((e) => currentKeySet.has(initiativeEntryKey(e)))
      .map((e) => {
        const key = initiativeEntryKey(e);
        const existing = currentEntries.find((ce) => ce.key === key);
        return existing ? { ...existing, entry: e, animationState: 'active' } : null;
      })
      .filter((e): e is InitiativeEntryState => e !== null);

    this.initiativeEntries.set(survivorsInNewOrder);

    // ── 3. Animate in entries that are newly added, one by one ──────────────
    const toAdd = newEntries.filter((e) => !currentKeySet.has(initiativeEntryKey(e)));

    for (const entry of toAdd) {
      if (this.isDestroyed) return;
      const key = initiativeEntryKey(entry);
      this.initiativeEntries.update((entries) => [
        ...entries,
        { entry, key, animationState: 'entering' },
      ]);
      await delay(ENTER_DURATION_MS);
      if (this.isDestroyed) return;
      this.initiativeEntries.update((entries) =>
        entries.map((e) => (e.key === key ? { ...e, animationState: 'active' } : e)),
      );
      await delay(ENTER_STAGGER_MS);
    }
  }

  private findLoggedInPlayer(players: PlayerGameAction[] | null): PlayerGameAction | null {
    if (!players || players.length === 0) {
      return null;
    }

    const currentUserId = this.authService.getCurrentUserId();
    if (!currentUserId) {
      return null;
    }

    return players.find((player) => player.userId === currentUserId) ?? null;
  }

  // ── Template helpers ─────────────────────────────────────────────────────

  protected readonly currentRoom = computed<RoomDto | null>(() => {
    const log = this.actionLog();
    for (let i = log.length - 1; i >= 0; i--) {
      if (log[i].room) return log[i].room ?? null;
    }
    return null;
  });

  protected readonly background = computed(() => {
    const backgroundId = this.currentRoom()?.backgroundId ?? 0;
    const dungeons = Assets.Images.Backgrounds.Dungeons;
    return dungeons[backgroundId % dungeons.length];
  });

  protected readonly Assets = Assets;
  protected readonly gameOverFadeDuration = `${GAME_OVER_FADE_MS}ms`;
  protected readonly gameOverTextFadeDuration = `${GAME_OVER_TEXT_FADE_MS}ms`;
  protected readonly gameOverButtonFadeDuration = `${GAME_OVER_BUTTON_FADE_MS}ms`;
  protected readonly roomWonFadeDuration = `${ROOM_WON_FADE_MS}ms`;
  protected readonly roomWonTextFadeDuration = `${ROOM_WON_TEXT_FADE_MS}ms`;
  protected readonly slotIndices = [0, 1, 2, 3];

  protected navigateHome(): void {
    void this.router.navigate(['']);
  }

  private readonly enemyByInstanceId = computed<Map<string, EnemyDto>>(
    () => new Map(this.displayEnemies().map((e) => [e.instanceId, e])),
  );

  protected initiativePortrait(entry: InitiativeDto): string {
    if (entry.type === InitiativeType.Enemy && entry.enemyInstanceId) {
      const enemy = this.enemyByInstanceId().get(entry.enemyInstanceId);
      if (enemy) {
        return `assets/images/enemies/${enemy.id}/portrait.png`;
      }
    }
    return Assets.Images.Characters.Silhouette;
  }

  protected partyMemberClassName(member: PartyMemberDto): string {
    return member.className?.toLowerCase() ?? '';
  }

  protected partyMemberIdleSprite(member: PartyMemberDto): string {
    const className = member.className?.toLowerCase();
    if (!className) {
      return Assets.Images.Characters.Placeholder;
    }
    const gender = member.gender === Gender.Male ? 'male' : 'female';
    return `assets/images/characters/${className}/${gender}/idle.webp`;
  }

  protected enemyIdleSprite(enemy: EnemyDto): string {
    if (this.deadEnemyInstanceIds().has(enemy.instanceId)) {
      const deadSprite = enemy.id
        ? `assets/images/enemies/${enemy.id}/dead.webp`
        : FALLBACK_DEAD_ENEMY_SPRITE;
      return `${deadSprite}?instanceId=${encodeURIComponent(enemy.instanceId)}`;
    }
    if (!enemy.id) {
      return Assets.Images.Characters.Placeholder;
    }
    return `assets/images/enemies/${enemy.id}/idle.webp`;
  }

  // ── Card hand helpers ────────────────────────────────────────────────────

  private readonly partyMemberById = computed<Map<number, PartyMemberDto>>(
    () => new Map(this.displayPartyMembers().map((m) => [m.partyMemberId, m])),
  );

  protected readonly activePlayerMana = computed(() => {
    const hand = this.displayHand();
    if (hand.length === 0) return this.displayMana();
    const member = this.partyMemberById().get(hand[0].partyMemberId);
    if (!member) {
      console.warn(`[Encounter] Party member not found for partyMemberId ${hand[0].partyMemberId}`);
      return this.displayMana();
    }
    return member.mana;
  });

  protected readonly onCardPlayed = (slotId: string): void => {
    const parts = slotId.split('-');
    const index = parseInt(parts[parts.length - 1], 10);
    const card = this.displayHand()[index];
    if (!card) {
      console.warn('[Encounter] Could not find card for slotId:', slotId);
      return;
    }
    this.handleCardPlay(card, slotId);
  };

  protected onCardDragStarted(): void {
    this.isAnyCardDragging.set(true);
  }

  protected onCardDragEnded(): void {
    this.isAnyCardDragging.set(false);
  }

  protected onCardPinned(slotId: string, card: CardDto): void {
    this.pinnedCardSlotId.set(slotId);
    this.pinnedCard.set({ ...card });
  }

  protected onDrawPileClick(): void {
    if (this.gameOverActive() || this.roomWonActive()) return;
    const id = this.gameId();
    if (id === null) return;
    if (this.activePile() === 'draw') {
      this.activePile.set(null);
      this.hoveredTooltipCard.set(null);
      return;
    }
    this.activePile.set('draw');
    this.hoveredTooltipCard.set(null);
    this.singlePlayerService
      .getDrawPile(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (result) => this.drawPileCards.set(result.cards ?? []),
        error: (err) => console.error('[Encounter] Failed to load draw pile', err),
      });
  }

  protected onDiscardPileClick(): void {
    if (this.gameOverActive() || this.roomWonActive()) return;
    const id = this.gameId();
    if (id === null) return;
    if (this.activePile() === 'discard') {
      this.activePile.set(null);
      this.hoveredTooltipCard.set(null);
      return;
    }
    this.activePile.set('discard');
    this.hoveredTooltipCard.set(null);
    this.singlePlayerService
      .getDiscardPile(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (result) => this.discardPileCards.set(result.cards ?? []),
        error: (err) => console.error('[Encounter] Failed to load discard pile', err),
      });
  }

  protected cardHandStyle(index: number, total: number): string {
    const spread = 12;
    const angle = (index - (total - 1) / 2) * spread;
    const translateY = Math.abs(angle) * 1.8;
    return `transform: rotate(${angle}deg) translateY(${translateY}px); z-index: ${total - Math.abs(index - (total - 1) / 2)};`;
  }

  protected partyMemberArmorDelta(member: PartyMemberDto): number | null {
    return this.partyMemberArmorBursts()[member.identifier]?.delta ?? null;
  }

  protected partyMemberArmorAnimationKey(member: PartyMemberDto): number {
    return this.partyMemberArmorBursts()[member.identifier]?.token ?? 0;
  }

  protected partyMemberArmorType(member: PartyMemberDto): ArmorPresentation {
    return this.partyMemberArmorPresentation()[member.identifier] ?? 'physical';
  }

  protected partyMemberStrengthDelta(member: PartyMemberDto): number | null {
    return this.partyMemberStrengthBursts()[member.identifier]?.delta ?? null;
  }

  protected partyMemberStrengthAnimationKey(member: PartyMemberDto): number {
    return this.partyMemberStrengthBursts()[member.identifier]?.token ?? 0;
  }

  protected partyMemberDefenseDelta(member: PartyMemberDto): number | null {
    return this.partyMemberDefenseBursts()[member.identifier]?.delta ?? null;
  }

  protected partyMemberDefenseAnimationKey(member: PartyMemberDto): number {
    return this.partyMemberDefenseBursts()[member.identifier]?.token ?? 0;
  }

  protected partyMemberSpeedDelta(member: PartyMemberDto): number | null {
    return this.partyMemberSpeedBursts()[member.identifier]?.delta ?? null;
  }

  protected partyMemberSpeedAnimationKey(member: PartyMemberDto): number {
    return this.partyMemberSpeedBursts()[member.identifier]?.token ?? 0;
  }

  protected partyMemberAttackAnimationKey(member: PartyMemberDto): number {
    return this.partyMemberAttackState()[member.identifier]?.token ?? 0;
  }

  protected partyMemberAttackingSprite(member: PartyMemberDto): string {
    const className = member.className?.toLowerCase();
    if (!className) return Assets.Images.Characters.Placeholder;
    const gender = member.gender === Gender.Male ? 'male' : 'female';
    return `assets/images/characters/${className}/${gender}/attacking.webp`;
  }

  protected partyMemberDefendAnimationKey(member: PartyMemberDto): number {
    return this.partyMemberDefendState()[member.identifier]?.token ?? 0;
  }

  protected partyMemberHealAnimationKey(member: PartyMemberDto): number {
    return (
      this.partyMemberHealState()[member.identifier]?.token ??
      this.partyMemberManaState()[member.identifier]?.token ??
      0
    );
  }

  protected partyMemberHealSprite(member: PartyMemberDto): string {
    if (this.partyMemberHealState()[member.identifier]) {
      return Assets.Animations.Heal;
    }
    return Assets.Animations.Mana;
  }

  protected partyMemberPoisonAnimationKey(member: PartyMemberDto): number {
    return this.partyMemberPoisonState()[member.identifier]?.token ?? 0;
  }

  protected partyMemberPoisonDelta(member: PartyMemberDto): number | null {
    return this.partyMemberPoisonState()[member.identifier]?.delta ?? null;
  }

  protected partyMemberDaggersAnimationKey(member: PartyMemberDto): number {
    return this.partyMemberDaggersAnimationState()[member.identifier] ?? 0;
  }

  protected partyMemberSpiritAnimationKey(member: PartyMemberDto): number {
    return this.partyMemberSpiritAnimationState()[member.identifier] ?? 0;
  }

  protected partyMemberElementAnimationKey(member: PartyMemberDto): number {
    return this.partyMemberElementAnimationState()[member.identifier] ?? 0;
  }

  protected partyMemberResourceDelta(member: PartyMemberDto): number | null {
    const healState = this.partyMemberHealState()[member.identifier];
    if (healState) {
      return healState.delta;
    }
    const manaState = this.partyMemberManaState()[member.identifier];
    if (manaState) {
      return manaState.delta;
    }
    return this.partyMemberDefendState()[member.identifier]?.delta ?? null;
  }

  protected partyMemberResourceDeltaTone(member: PartyMemberDto): 'default' | 'healing' {
    if (
      this.partyMemberManaState()[member.identifier] ||
      this.partyMemberHealState()[member.identifier]
    ) {
      return 'healing';
    }
    const delta = this.partyMemberDefendState()[member.identifier]?.delta ?? 0;
    return delta > 0 ? 'healing' : 'default';
  }

  protected partyMemberDefendingSprite(member: PartyMemberDto): string {
    const className = member.className?.toLowerCase();
    if (!className) return Assets.Images.Characters.Placeholder;
    let gender: 'male' | 'female' | null = null;
    if (member.gender === Gender.Male) {
      gender = 'male';
    } else if (member.gender === Gender.Female) {
      gender = 'female';
    }
    if (!gender) return Assets.Images.Characters.Placeholder;
    return `assets/images/characters/${className}/${gender}/defending.png`;
  }

  protected enemyDefendAnimationKey(enemy: EnemyDto): number {
    return this.enemyDefendState()[enemy.instanceId]?.token ?? 0;
  }

  protected enemyArmorDelta(enemy: EnemyDto): number | null {
    return this.enemyArmorBursts()[enemy.instanceId]?.delta ?? null;
  }

  protected enemyArmorAnimationKey(enemy: EnemyDto): number {
    return this.enemyArmorBursts()[enemy.instanceId]?.token ?? 0;
  }

  protected enemyStrengthDelta(enemy: EnemyDto): number | null {
    return this.enemyStrengthBursts()[enemy.instanceId]?.delta ?? null;
  }

  protected enemyStrengthAnimationKey(enemy: EnemyDto): number {
    return this.enemyStrengthBursts()[enemy.instanceId]?.token ?? 0;
  }

  protected enemyDefenseDelta(enemy: EnemyDto): number | null {
    return this.enemyDefenseBursts()[enemy.instanceId]?.delta ?? null;
  }

  protected enemyDefenseAnimationKey(enemy: EnemyDto): number {
    return this.enemyDefenseBursts()[enemy.instanceId]?.token ?? 0;
  }

  protected enemySpeedDelta(enemy: EnemyDto): number | null {
    return this.enemySpeedBursts()[enemy.instanceId]?.delta ?? null;
  }

  protected enemySpeedAnimationKey(enemy: EnemyDto): number {
    return this.enemySpeedBursts()[enemy.instanceId]?.token ?? 0;
  }

  protected enemyAttackAnimationKey(enemy: EnemyDto): number {
    return this.enemyAttackState()[enemy.instanceId]?.token ?? 0;
  }

  protected enemyAttackingSprite(enemy: EnemyDto): string {
    if (!enemy.id) return Assets.Images.Characters.Placeholder;
    return `assets/images/enemies/${enemy.id}/attacking.webp`;
  }

  protected enemyHealingAnimationKey(enemy: EnemyDto): number {
    return this.enemyHealState()[enemy.instanceId]?.token ?? 0;
  }

  protected enemyHealingSprite(): string {
    return Assets.Animations.Heal;
  }

  protected enemyHpDelta(enemy: EnemyDto): number | null {
    return (
      this.enemyHealState()[enemy.instanceId]?.delta ??
      this.enemyDefendState()[enemy.instanceId]?.delta ??
      null
    );
  }

  protected enemyHpDeltaTone(enemy: EnemyDto): 'default' | 'healing' {
    return this.enemyHealState()[enemy.instanceId] ? 'healing' : 'default';
  }

  protected enemyPoisonAnimationKey(enemy: EnemyDto): number {
    return this.enemyPoisonState()[enemy.instanceId]?.token ?? 0;
  }

  protected enemyPoisonDelta(enemy: EnemyDto): number | null {
    return this.enemyPoisonState()[enemy.instanceId]?.delta ?? null;
  }

  protected enemyDaggersAnimationKey(enemy: EnemyDto): number {
    return this.enemyDaggersAnimationState()[enemy.instanceId] ?? 0;
  }

  protected enemyDefendingSprite(enemy: EnemyDto): string {
    if (!enemy.id) return Assets.Images.Characters.Placeholder;
    return `assets/images/enemies/${enemy.id}/defending.png`;
  }

  // ── Target selection ──────────────────────────────────────────────────────

  /** Which phase of target selection is active, or null if no card is pending. */
  protected readonly targetSelectionPhase = computed<'party' | 'enemy' | null>(() => {
    const pending = this.pendingCardPlay();
    if (!pending) return null;
    if (pending.remainingPartyTargets > 0) return 'party';
    if (pending.remainingEnemyTargets > 0) return 'enemy';
    return null;
  });

  /** True when this party member should be highlighted for selection. */
  protected isPartyMemberSelectable(member: PartyMemberDto): boolean {
    return (
      this.targetSelectionPhase() === 'party' &&
      !this.deadPartyMemberIdentifiers().has(member.identifier)
    );
  }

  /** True when a living enemy character should be highlighted for selection. */
  protected isEnemySelectable(enemy: EnemyDto): boolean {
    return (
      this.targetSelectionPhase() === 'enemy' && !this.deadEnemyInstanceIds().has(enemy.instanceId)
    );
  }

  /** Whether cards can currently be dragged and played (blocked during target selection). */
  protected readonly canPlayCards = computed(
    () =>
      this.isMyTurn() &&
      this.pendingCardPlay() === null &&
      !this.gameOverActive() &&
      !this.roomWonActive(),
  );

  /** Set of party member identifiers (UUIDs) that have been selected as targets. */
  protected readonly selectedPartyMemberIdentifiers = computed(
    () => new Set(this.pendingCardPlay()?.selectedPartyMemberInstanceIds ?? []),
  );

  /** Set of enemy instance IDs (UUIDs) that have been selected as targets. */
  protected readonly selectedEnemyInstanceIdSet = computed(
    () => new Set(this.pendingCardPlay()?.selectedEnemyInstanceIds ?? []),
  );

  /** Portrait URLs and border colors for ally targets already selected. */
  protected readonly selectedPartyPortraits = computed(() => {
    const pending = this.pendingCardPlay();
    if (!pending) return [];
    return pending.selectedPartyMemberInstanceIds.map((id) => {
      const member = this.displayPartyMembers().find((m) => m.identifier === id);
      return {
        portraitUrl: member
          ? this.partyMemberPortrait(member)
          : Assets.Images.Characters.Silhouette,
        borderColor: member?.color ?? '#ffd700',
      };
    });
  });

  /** Portrait URLs and border colors for enemy targets already selected. */
  protected readonly selectedEnemyPortraits = computed(() => {
    const pending = this.pendingCardPlay();
    if (!pending) return [];
    return pending.selectedEnemyInstanceIds.map((id) => {
      const enemy = this.displayEnemies().find((e) => e.instanceId === id);
      return {
        portraitUrl: enemy
          ? `assets/images/enemies/${enemy.id}/portrait.png`
          : Assets.Images.Characters.Silhouette,
        borderColor: '#ffd700',
      };
    });
  });

  /** Hint text shown during target selection. */
  protected readonly targetSelectionHint = computed(() => {
    const pending = this.pendingCardPlay();
    if (!pending) return null;
    const phase = this.targetSelectionPhase();
    if (phase === 'party') {
      return `Select ${pending.remainingPartyTargets} ally target${pending.remainingPartyTargets !== 1 ? 's' : ''}`;
    }
    if (phase === 'enemy') {
      return `Select ${pending.remainingEnemyTargets} enemy target${pending.remainingEnemyTargets !== 1 ? 's' : ''}`;
    }
    return null;
  });

  protected partyMemberPortrait(member: PartyMemberDto): string {
    const className = member.className?.toLowerCase();
    if (!className) return Assets.Images.Characters.Placeholder;
    const gender = member.gender === Gender.Male ? 'male' : 'female';
    return `assets/images/characters/${className}/${gender}/portrait.webp`;
  }

  protected getBoardCardImage(card: CardDto): string {
    const role = CardDescriptionHelper.resolveRole(card.classId);
    if (!role || !card.cardId) {
      return Assets.Images.Cards.Default;
    }
    return `assets/images/cards/${role}/${card.cardId}.webp`;
  }

  protected onCardHover(card: CardDto): void {
    if (this.isAnyCardDragging()) return;
    this.hoveredTooltipCard.set(card);
  }

  protected onCardLeave(): void {
    this.hoveredTooltipCard.set(null);
  }

  protected onPartyMemberClick(member: PartyMemberDto): void {
    if (this.gameOverActive() || this.roomWonActive()) return;
    const pending = this.pendingCardPlay();
    if (!pending || !this.isPartyMemberSelectable(member)) return;

    const newSelected = [...pending.selectedPartyMemberInstanceIds, member.identifier];
    const remainingParty = pending.remainingPartyTargets - 1;
    const updated: PendingCardPlay = {
      ...pending,
      selectedPartyMemberInstanceIds: newSelected,
      remainingPartyTargets: remainingParty,
    };

    if (remainingParty === 0 && updated.remainingEnemyTargets === 0) {
      this.pendingCardPlay.set(null);
      this.submitCardPlay(updated);
    } else {
      this.pendingCardPlay.set(updated);
    }
  }

  protected onEnemyClick(enemy: EnemyDto): void {
    if (this.gameOverActive() || this.roomWonActive()) return;
    const pending = this.pendingCardPlay();
    if (!pending || this.targetSelectionPhase() !== 'enemy') return;
    if (this.deadEnemyInstanceIds().has(enemy.instanceId)) return;

    const newSelected = [...pending.selectedEnemyInstanceIds, enemy.instanceId];
    const remainingEnemies = pending.remainingEnemyTargets - 1;
    const updated: PendingCardPlay = {
      ...pending,
      selectedEnemyInstanceIds: newSelected,
      remainingEnemyTargets: remainingEnemies,
    };

    if (remainingEnemies === 0 && updated.remainingPartyTargets === 0) {
      this.pendingCardPlay.set(null);
      this.submitCardPlay(updated);
    } else {
      this.pendingCardPlay.set(updated);
    }
  }

  protected cancelTargetSelection(): void {
    this.pendingCardPlay.set(null);
    this.pinnedCard.set(null);
    this.pinnedCardSlotId.set(null);
    this.hoveredTooltipCard.set(null);
  }

  private handleCardPlay(card: CardDto, slotId?: string): void {
    const gameId = this.gameId();
    if (gameId === null) return;

    const playerTargets = card.playerTargets ?? 0;
    const enemyTargets = card.enemyTargets ?? 0;

    if (playerTargets === 0 && enemyTargets === 0) {
      this.submitPlayCardRequest(gameId, {
        cardInstanceId: card.instanceId,
        partyMemberInstanceIds: [],
        enemyInstanceIds: [],
      });
    } else {
      // For the direct drop-zone play path pinnedCard/pinnedCardSlotId are not yet
      // set (they are only set by onCardPinned for the center-zone pin path).
      // Set them here so that:
      //   • the card thumbnail appears in the center zone during target selection
      //   • the cancel button and hint text render (both are inside @if(pinnedCard()))
      //   • the card component in the hand is destroyed (via pinnedCardSlotId) so
      //     that cancelling recreates it with full opacity instead of leaving it stuck
      //     at opacity-0 from the fadeOutAnimation.
      if (!this.pinnedCard()) {
        this.pinnedCard.set({ ...card });
        if (slotId) {
          this.pinnedCardSlotId.set(slotId);
        }
      }
      this.pendingCardPlay.set({
        card,
        remainingPartyTargets: playerTargets,
        remainingEnemyTargets: enemyTargets,
        selectedPartyMemberInstanceIds: [],
        selectedEnemyInstanceIds: [],
      });
    }
  }

  private submitCardPlay(pending: PendingCardPlay): void {
    const gameId = this.gameId();
    if (gameId === null) return;

    this.submitPlayCardRequest(gameId, {
      cardInstanceId: pending.card.instanceId,
      partyMemberInstanceIds: pending.selectedPartyMemberInstanceIds,
      enemyInstanceIds: pending.selectedEnemyInstanceIds,
    });
  }

  private submitPlayCardRequest(gameId: number, request: PlayCardRequest): void {
    // Snapshot the played card for the static center-zone display if it hasn't
    // been pinned there already (e.g. direct drop-zone plays).
    const playedCard =
      this.pinnedCard() ?? this.displayHand().find((c) => c.instanceId === request.cardInstanceId);
    this.inFlightPlayedCard.set(playedCard ?? null);
    if (playedCard && !this.pinnedCard()) {
      this.pinnedCard.set({ ...playedCard });
    }

    // Remove the played card from the hand immediately so it cannot be
    // re-created at the hand position while the server request is in-flight.
    this.displayHand.update((hand) => hand.filter((c) => c.instanceId !== request.cardInstanceId));
    // pinnedCardSlotId is no longer needed once the card is removed from the hand.
    this.pinnedCardSlotId.set(null);
    this.hoveredTooltipCard.set(null);

    this.singlePlayerService
      .playCard(gameId, request)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (actions) => {
          this.actionsReceived.emit(actions);
        },
        error: (err) => {
          console.error('[Encounter] Failed to play card', err);
          this.restoreInFlightPlayedCardToHand();
          this.clearCardPlayState();
        },
      });
  }

  private restoreInFlightPlayedCardToHand(): void {
    const inFlight = this.inFlightPlayedCard();
    if (!inFlight) {
      return;
    }

    this.displayHand.update((hand) => {
      if (hand.some((card) => card.instanceId === inFlight.instanceId)) {
        return hand;
      }
      return [...hand, inFlight];
    });
    this.inFlightPlayedCard.set(null);
  }

  private clearCardPlayState(): void {
    this.pinnedCard.set(null);
    this.pinnedCardSlotId.set(null);
    this.pendingCardPlay.set(null);
    this.hoveredTooltipCard.set(null);
  }
}
