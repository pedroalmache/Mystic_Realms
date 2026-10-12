import {
  AfterViewInit,
  Component,
  DestroyRef,
  OnDestroy,
  computed,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';

import {
  CHEST_FADE_MS,
  CHEST_FLASH_MS,
  CHEST_IDLE_DELAY_MS,
  CHEST_OPEN_HOLD_MS,
  CHEST_REWARD_POP_ITEM_DURATION_MS,
  CHEST_REWARD_POP_ITEM_STAGGER_MS,
  CHEST_REWARD_POP_TOTAL_MS,
  CHEST_SHAKE_BURST_MS,
  CHEST_SHAKE_PAUSE_MS,
  CHEST_SHAKE_PROGRESSIVE_DELAY_MS,
  ROOM_REWARDS_INTRO_FADE_DELAY_MS,
} from '../../core/constants/animation.constants';
import { Assets } from '../../core/constants/assets.constants';
import { AuthService } from '../../core/auth/auth.service';
import {
  BlessingDto,
  CardDto,
  ConfirmRewardItem,
  GameAction,
  Gender,
  PartyMemberDto,
  RewardType,
  RewardPartyMemberDto,
  RewardPlayerDto,
  RoomRewardsDto,
  SinglePlayerService,
} from '../../core/services/single-player.service';
import { CardDisplayComponent } from '../../shared/card-display/card-display';
import { CardTooltipComponent } from '../../shared/card-tooltip/card-tooltip';

export type ChestPhase =
  | 'idle'
  | 'shake-soft'
  | 'shake-medium'
  | 'shake-hard'
  | 'flash'
  | 'open'
  | 'fade'
  | 'done';
export type RewardStage = 'chest' | 'reward-pop' | 'rewards' | 'voting' | 'blessings';

@Component({
  selector: 'app-room-rewards',
  templateUrl: './room-rewards.html',
  styleUrl: './room-rewards.scss',
  imports: [CardDisplayComponent, CardTooltipComponent],
})
export class RoomRewardsComponent implements AfterViewInit, OnDestroy {
  private static readonly DECK_CARD_MAX_USE_COUNT = 20;
  private static readonly PERCENTAGE_MULTIPLIER = 100;

  readonly gameId = input<number | null>(null);
  readonly actionLog = input<GameAction[]>([]);
  readonly rewardConfirmed = output<void>();

  private readonly singlePlayerService = inject(SinglePlayerService);
  private readonly authService = inject(AuthService);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly Assets = Assets;
  protected readonly background = Assets.Images.Backgrounds.Rewards;
  protected readonly rewardCardBackIcon = Assets.Images.Cards.Display.CardBack;
  protected readonly chestRewardPopItemDurationMs = CHEST_REWARD_POP_ITEM_DURATION_MS;
  protected readonly chestRewardPopItemStaggerMs = CHEST_REWARD_POP_ITEM_STAGGER_MS;

  protected readonly showIntroBlack = signal(true);
  protected readonly chestPhase = signal<ChestPhase>('idle');
  protected readonly stage = signal<RewardStage>('chest');
  protected readonly isLoading = signal(true);
  protected readonly hasError = signal(false);
  protected readonly rewards = signal<RoomRewardsDto | null>(null);

  /** Map from playerId → selected cardInstanceId, or 'skip'. */
  protected readonly cardSelections = signal<Record<string, string>>({});
  /** PartyMemberId voted to receive the blessing, or null. */
  protected readonly votedMemberId = signal<number | null>(null);
  /** BlessingId selected in the blessings stage, or null. */
  protected readonly selectedBlessingId = signal<number | null>(null);
  /** True while confirming the selected blessing with the backend. */
  protected readonly isConfirmingReward = signal(false);
  /** True when confirming reward failed. */
  protected readonly hasConfirmRewardError = signal(false);
  /** The card currently being hovered in the rewards stage. */
  protected readonly hoveredCard = signal<CardDto | null>(null);
  protected readonly isDeckOverlayOpen = signal(false);
  protected readonly selectedDeckPlayerTag = signal<string>('Player');
  protected readonly deckCards = signal<CardDto[]>([]);
  protected readonly isDeckLoading = signal(false);
  protected readonly hasDeckError = signal(false);
  protected readonly deckCardMaxUseCount = RoomRewardsComponent.DECK_CARD_MAX_USE_COUNT;
  protected readonly percentageMultiplier = RoomRewardsComponent.PERCENTAGE_MULTIPLIER;
  protected readonly deckHoveredCard = signal<CardDto | null>(null);

  private isDestroyed = false;
  private introFadeTimeout: ReturnType<typeof setTimeout> | null = null;
  private chestTimeout: ReturnType<typeof setTimeout> | null = null;
  private deckRequestId = 0;

  /** Builds a map from partyMemberId → PartyMemberDto using the action log. */
  protected readonly partyMemberInfoMap = computed<Map<number, PartyMemberDto>>(() => {
    const map = new Map<number, PartyMemberDto>();
    for (const action of this.actionLog()) {
      for (const m of action.partyMembers ?? []) {
        if (!map.has(m.partyMemberId)) {
          map.set(m.partyMemberId, m);
        }
      }
    }
    return map;
  });

  /** Builds a map from partyMemberId → reward payload member info. */
  protected readonly rewardMemberInfoMap = computed<Map<number, RewardPartyMemberDto>>(() => {
    const map = new Map<number, RewardPartyMemberDto>();
    for (const player of this.rewards()?.players ?? []) {
      for (const m of player.partyMembers ?? []) {
        if (!map.has(m.partyMemberId)) {
          map.set(m.partyMemberId, m);
        }
      }
    }
    return map;
  });

  /** The current player's section of the reward response. */
  protected readonly currentPlayerReward = computed(() => {
    const r = this.rewards();
    if (!r) return null;
    const userId = this.authService.getCurrentUserId();
    if (!userId) return r.players[0] ?? null;
    return r.players.find((p) => p.userId === userId) ?? r.players[0] ?? null;
  });

  /** Number of reward cards the current player receives. */
  protected readonly currentPlayerCardCount = computed(() => {
    return this.currentPlayerReward()?.cards.length ?? 0;
  });

  /** Sum of all gold rewards from the current player's party members. */
  protected readonly currentPlayerTotalGold = computed(() => {
    const partyMembers = this.currentPlayerReward()?.partyMembers ?? [];
    return partyMembers.reduce((sum, member) => sum + (member.gold ?? 0), 0);
  });

  /** Unique partyMemberIds across all players' cards (for the voting stage). */
  protected readonly allRewardMemberIds = computed<number[]>(() => {
    const seen = new Set<number>();
    const result: number[] = [];
    for (const player of (this.rewards()?.players ?? [])) {
      for (const member of player.partyMembers ?? []) {
        if (!seen.has(member.partyMemberId)) {
          seen.add(member.partyMemberId);
          result.push(member.partyMemberId);
        }
      }
    }
    return result;
  });

  /** True once the current player has made an explicit card selection or skip. */
  protected readonly canConfirmSelections = computed(() => {
    const player = this.currentPlayerReward();
    if (!player) return false;
    if (player.cards.length === 0) return true;
    const key = this.getSelectionKey(player.playerId);
    return this.cardSelections()[key] !== undefined;
  });

  /** True once the current player has made any card/skip selection. */
  protected readonly hasAnySelection = computed(() => {
    const player = this.currentPlayerReward();
    if (!player || player.cards.length === 0) return false;
    const key = this.getSelectionKey(player.playerId);
    return this.cardSelections()[key] !== undefined;
  });

  /** TypeIcon URL derived from the first blessing's type. */
  protected readonly blessingTypeIconUrl = computed(() => {
    const blessings = this.rewards()?.blessings ?? [];
    if (blessings.length === 0) return null;
    return this.getBlessingTypeIcon(blessings[0].type);
  });

  ngAfterViewInit(): void {
    this.introFadeTimeout = setTimeout(() => {
      this.showIntroBlack.set(false);
      this.introFadeTimeout = null;
    }, ROOM_REWARDS_INTRO_FADE_DELAY_MS);

    // Fetch rewards in parallel with the chest animation.
    const gameId = this.gameId();
    if (gameId != null) {
      this.singlePlayerService
        .getRoomRewards(gameId)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (data) => {
            this.rewards.set(data);
            this.cardSelections.set({});
            this.selectedBlessingId.set(null);
            this.isConfirmingReward.set(false);
            this.hasConfirmRewardError.set(false);
            this.isLoading.set(false);
          },
          error: (err) => {
            console.error('[RoomRewards] Failed to fetch rewards', err);
            this.hasError.set(true);
            this.isLoading.set(false);
          },
        });
    } else {
      this.isLoading.set(false);
    }

    this.runChestAnimation();
  }

  ngOnDestroy(): void {
    this.isDestroyed = true;
    if (this.introFadeTimeout !== null) {
      clearTimeout(this.introFadeTimeout);
      this.introFadeTimeout = null;
    }
    if (this.chestTimeout !== null) {
      clearTimeout(this.chestTimeout);
      this.chestTimeout = null;
    }
  }

  // ── Chest animation ────────────────────────────────────────────────────────

  private after(ms: number): Promise<void> {
    return new Promise((resolve) => {
      this.chestTimeout = setTimeout(() => {
        this.chestTimeout = null;
        resolve();
      }, ms);
    });
  }

  private async runChestAnimation(): Promise<void> {
    await this.after(CHEST_IDLE_DELAY_MS);
    if (this.isDestroyed) return;

    const shakePhases: ChestPhase[] = ['shake-soft', 'shake-medium', 'shake-hard'];
    for (let i = 0; i < shakePhases.length; i++) {
      this.chestPhase.set(shakePhases[i]);
      await this.after(CHEST_SHAKE_BURST_MS + i * CHEST_SHAKE_PROGRESSIVE_DELAY_MS);
      if (this.isDestroyed) return;

      this.chestPhase.set('idle');
      await this.after(CHEST_SHAKE_PAUSE_MS);
      if (this.isDestroyed) return;
    }

    this.chestPhase.set('flash');
    await this.after(CHEST_FLASH_MS);
    if (this.isDestroyed) return;

    this.chestPhase.set('open');
    await this.after(CHEST_OPEN_HOLD_MS);
    if (this.isDestroyed) return;

    this.stage.set('reward-pop');
    await this.after(CHEST_REWARD_POP_TOTAL_MS);
    if (this.isDestroyed) return;

    this.chestPhase.set('fade');
    await this.after(CHEST_FADE_MS);
    if (this.isDestroyed) return;

    this.chestPhase.set('done');
    this.stage.set('rewards');
  }

  // ── Card selection ─────────────────────────────────────────────────────────

  /** Returns the unique partyMemberIds that appear in a player's cards. */
  protected getUniqueMemberIds(player: RewardPlayerDto): number[] {
    return [...new Set(player.partyMembers.map((m) => m.partyMemberId))];
  }

  protected getMemberGold(player: RewardPlayerDto, partyMemberId: number): number {
    return player.partyMembers.find((m) => m.partyMemberId === partyMemberId)?.gold ?? 0;
  }

  private getSelectionKey(playerId: number): string {
    return String(playerId);
  }

  protected selectCard(playerId: number, cardInstanceId: string): void {
    const key = this.getSelectionKey(playerId);
    this.cardSelections.update((prev) => ({ ...prev, [key]: cardInstanceId }));
  }

  protected skipCard(playerId: number): void {
    const key = this.getSelectionKey(playerId);
    this.cardSelections.update((prev) => ({ ...prev, [key]: 'skip' }));
  }

  protected getCardSelection(playerId: number): string | undefined {
    return this.cardSelections()[this.getSelectionKey(playerId)];
  }

  protected hoverCard(card: CardDto): void {
    this.hoveredCard.set(card);
  }

  protected clearHoveredCard(): void {
    this.hoveredCard.set(null);
  }

  protected openPlayerDeck(player: RewardPlayerDto): void {
    this.isDeckOverlayOpen.set(true);
    this.deckCards.set([]);
    this.deckHoveredCard.set(null);
    this.isDeckLoading.set(true);
    this.hasDeckError.set(false);
    this.selectedDeckPlayerTag.set(this.getPlayerDisplayName(player));

    const gameId = this.gameId();
    if (gameId == null) {
      this.isDeckLoading.set(false);
      this.hasDeckError.set(true);
      return;
    }

    const requestId = ++this.deckRequestId;

    this.singlePlayerService
      .getPlayerDeck(gameId, player.playerId)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          if (requestId === this.deckRequestId) {
            this.isDeckLoading.set(false);
          }
        }),
      )
      .subscribe({
        next: (response) => {
          if (requestId !== this.deckRequestId) return;
          this.deckCards.set(response.cards ?? []);
        },
        error: (err) => {
          if (requestId !== this.deckRequestId) return;
          console.error('[RoomRewards] Failed to fetch player deck', err);
          this.hasDeckError.set(true);
          this.deckCards.set([]);
        },
      });
  }

  protected closeDeckOverlay(): void {
    this.deckRequestId += 1;
    this.isDeckLoading.set(false);
    this.deckHoveredCard.set(null);
    this.isDeckOverlayOpen.set(false);
  }

  protected hoverDeckCard(card: CardDto): void {
    this.deckHoveredCard.set(card);
  }

  protected clearDeckHoveredCard(): void {
    this.deckHoveredCard.set(null);
  }

  protected getDeckCardUseProgress(useCount: number): number {
    const safeUseCount = Number.isFinite(useCount) ? useCount : 0;
    const clamped = Math.max(0, Math.min(this.deckCardMaxUseCount, safeUseCount));
    return (clamped / this.deckCardMaxUseCount) * this.percentageMultiplier;
  }

  protected getPlayerDisplayName(player: RewardPlayerDto): string {
    return player.gamerTag ?? player.userId ?? 'Player';
  }

  protected confirmSelections(): void {
    if (!this.canConfirmSelections()) return;
    this.hoveredCard.set(null);
    this.stage.set('voting');
  }

  // ── Voting ─────────────────────────────────────────────────────────────────

  protected voteForMember(partyMemberId: number): void {
    this.votedMemberId.set(partyMemberId);
  }

  protected confirmVote(): void {
    if (this.votedMemberId() === null) return;
    this.hoveredCard.set(null);
    this.stage.set('blessings');
  }

  protected selectBlessing(blessingId: number): void {
    if (this.isConfirmingReward()) return;

    this.hoveredCard.set(null);
    this.selectedBlessingId.set(blessingId);

    const gameId = this.gameId();
    if (gameId == null) return;

    const player = this.currentPlayerReward();
    const payload: ConfirmRewardItem[] = [];
    if (player) {
      const selection = this.getCardSelection(player.playerId);
      if (selection && selection !== 'skip') {
        const card = player.cards.find((c) => c.instanceId === selection);
        if (card) {
          payload.push({ partyMemberId: card.partyMemberId, cardId: card.cardId });
        }
      }
    }

    this.isConfirmingReward.set(true);
    this.hasConfirmRewardError.set(false);

    this.singlePlayerService
      .confirmReward(gameId, payload)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.isConfirmingReward.set(false)),
      )
      .subscribe({
        next: () => {
          this.rewardConfirmed.emit();
        },
        error: (err) => {
          console.error('[RoomRewards] Failed to confirm reward', err);
          this.selectedBlessingId.set(null);
          this.hasConfirmRewardError.set(true);
        },
      });
  }

  // ── Portrait / color helpers ───────────────────────────────────────────────

  protected getMemberPortrait(partyMemberId: number): string {
    const rewardInfo = this.rewardMemberInfoMap().get(partyMemberId);
    const info = this.partyMemberInfoMap().get(partyMemberId);
    const className = rewardInfo?.className ?? info?.className;
    const gender = rewardInfo?.gender ?? info?.gender;

    if (!className || gender == null) {
      return Assets.Images.Characters.Placeholder;
    }
    const genderFolder = gender === Gender.Male ? 'male' : 'female';
    return `assets/images/characters/${className.toLowerCase()}/${genderFolder}/portrait.webp`;
  }

  protected getMemberColor(partyMemberId: number): string {
    return this.rewardMemberInfoMap().get(partyMemberId)?.color
      ?? this.partyMemberInfoMap().get(partyMemberId)?.color
      ?? '#888';
  }

  protected getMemberName(partyMemberId: number): string {
    const rewardInfo = this.rewardMemberInfoMap().get(partyMemberId);
    const info = this.partyMemberInfoMap().get(partyMemberId);
    return rewardInfo?.className ?? info?.className ?? 'Unknown';
  }

  // ── Blessing helpers ───────────────────────────────────────────────────────

  protected getBlessingFolder(type: number): string {
    switch (type) {
      case RewardType.Defensive: return 'defensive';
      case RewardType.Gold: return 'gold';
      case RewardType.Health: return 'health';
      case RewardType.Mana: return 'mana';
      case RewardType.Misc: return 'misc';
      case RewardType.Offensive: return 'offensive';
      case RewardType.Speed: return 'speed';
      default: return 'misc';
    }
  }

  protected getBlessingTypeIcon(type: number): string {
    const folder = this.getBlessingFolder(type);
    const ext = folder === 'misc' ? 'png' : 'webp';
    return `assets/images/blessings/${folder}/type.${ext}`;
  }

  protected getBlessingImage(blessing: BlessingDto): string {
    return `assets/images/blessings/${this.getBlessingFolder(blessing.type)}/${blessing.blessingId}.PNG`;
  }

  protected getBlessingRarityColor(rarity: number): string {
    const clamped = Math.max(1, Math.min(5, rarity));
    switch (clamped) {
      case 1: return '#4aa3ff'; // blue
      case 2: return '#4bd16f'; // green
      case 3: return '#f0c040'; // yellow
      case 4: return '#b47cff'; // purple
      case 5: return '#ff4b4b'; // red
      default: return '#f0c040';
    }
  }

  protected isCurrentPlayer(player: RewardPlayerDto): boolean {
    const current = this.currentPlayerReward();
    if (!current) return false;
    return current.playerId === player.playerId;
  }
}
