import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DestroyRef } from '@angular/core';

import { Assets } from '../../core/constants/assets.constants';
import { CharactersService, CharacterResult } from '../../core/services/characters.service';
import { SinglePlayerService, CardDto } from '../../core/services/single-player.service';
import { CardDisplayComponent } from '../../shared/card-display/card-display';
import { CardTooltipComponent } from '../../shared/card-tooltip/card-tooltip';

interface PartyMember {
  classIndex: number;
  gender: 'male' | 'female';
}

@Component({
  selector: 'app-character-selection',
  templateUrl: './character-selection.html',
  styleUrl: './character-selection.scss',
  imports: [CardDisplayComponent, CardTooltipComponent],
})
export class CharacterSelectionComponent implements OnInit {
  private readonly router = inject(Router);
  private readonly charactersService = inject(CharactersService);
  private readonly singlePlayerService = inject(SinglePlayerService);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly background = Assets.Images.Backgrounds.CharacterSelection;
  protected readonly topTitleBg = Assets.Images.UI.TopTitleBg;
  protected readonly greenButton = Assets.Images.UI.GreenButton;
  protected readonly maleIcon = Assets.Images.Icons.Male;
  protected readonly femaleIcon = Assets.Images.Icons.Female;
  protected readonly hpIcon = Assets.Images.Icons.Hp;
  protected readonly manaIcon = Assets.Images.Icons.Mana;
  protected readonly speedIcon = Assets.Images.Icons.Speed;
  protected readonly roleIconEmpty = Assets.Images.Icons.RoleIconEmpty;
  protected readonly roleIconFrame = Assets.Images.UI.RoleIconFrame;
  protected readonly roleIconFrameFocus = Assets.Images.UI.RoleIconFrameFocus;
  protected readonly characterFrame = Assets.Images.UI.CharacterFrame.Frame;
  protected readonly characterFrameBlueBg = Assets.Images.UI.CharacterFrame.BlueBg;
  protected readonly textField = Assets.Images.UI.TextField;
  protected readonly lockIcon = Assets.Images.UI.LockIcon;
  protected readonly deckIcon = Assets.Images.Icons.Deck;
  protected readonly closeIcon = Assets.Images.UI.CloseButton;

  protected readonly classes = signal<CharacterResult[]>([]);
  protected readonly isLoading = signal<boolean>(true);
  protected readonly hasError = signal<boolean>(false);
  protected readonly isStarting = signal<boolean>(false);
  protected readonly selectedIndex = signal<number>(0);
  protected readonly selectedGender = signal<'male' | 'female'>('male');

  protected readonly partySlots = signal<Array<PartyMember | null>>([null, null, null, null]);
  protected readonly slotColors = signal<string[]>(['#e74c3c', '#3498db', '#2ecc71', '#f39c12']);
  protected readonly activePartySlot = signal<number>(0);
  protected readonly gameSeed = signal<string>('');

  protected readonly isDeckOverlayOpen = signal<boolean>(false);
  protected readonly isDeckLoading = signal<boolean>(false);
  protected readonly deckCards = signal<CardDto[]>([]);
  protected readonly hasDeckError = signal<boolean>(false);
  protected readonly hoveredCard = signal<CardDto | null>(null);
  protected readonly tooltipTop = signal<number>(-105);
  protected readonly tooltipLeft = signal<number>(0);

  protected readonly selectedCharacter = computed(() => this.classes()[this.selectedIndex()]);

  protected readonly canStart = computed(() => this.partySlots().some((s) => s !== null));

  protected readonly malePortrait = computed(() => {
    const cls = this.selectedCharacter();
    if (!cls) return Assets.Images.Characters.Silhouette;
    return `assets/images/characters/${cls.name.toLowerCase()}/male/portrait.webp`;
  });

  protected readonly femalePortrait = computed(() => {
    const cls = this.selectedCharacter();
    if (!cls) return Assets.Images.Characters.Silhouette;
    return `assets/images/characters/${cls.name.toLowerCase()}/female/portrait.webp`;
  });

  protected readonly characterImage = computed(() => {
    return this.selectedGender() === 'male' ? this.malePortrait() : this.femalePortrait();
  });

  protected readonly portraitImage = computed(() => {
    const cls = this.selectedCharacter();
    if (!cls) return Assets.Images.Characters.Placeholder;
    const gender = this.selectedGender();
    return `assets/images/characters/${cls.name.toLowerCase()}/${gender}/front_idle.webp`;
  });

  ngOnInit(): void {
    this.charactersService
      .getCharacters()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (characters) => {
          this.classes.set(characters);
          if (characters.length > 0) {
            this.selectedIndex.set(0);
          }
          this.isLoading.set(false);
        },
        error: (err) => {
          console.error('Failed to load characters', err);
          this.hasError.set(true);
          this.isLoading.set(false);
        },
      });
  }

  protected selectCharacter(index: number): void {
    this.selectedIndex.set(index);
  }

  protected selectGender(gender: 'male' | 'female'): void {
    this.selectedGender.set(gender);
  }

  protected selectPartySlot(slot: number): void {
    this.activePartySlot.set(slot);
    const member = this.partySlots()[slot];
    if (member) {
      this.selectedIndex.set(member.classIndex);
      this.selectedGender.set(member.gender);
    }
  }

  protected onSelect(): void {
    const slots = [...this.partySlots()];
    const current = this.activePartySlot();
    slots[current] = {
      classIndex: this.selectedIndex(),
      gender: this.selectedGender(),
    };
    this.partySlots.set(slots);

    const next = slots.findIndex((s, i) => i > current && s === null);
    if (next !== -1) {
      this.activePartySlot.set(next);
    } else if (slots.some((s) => s === null)) {
      // wrap around to the first empty slot
      const first = slots.findIndex((s) => s === null);
      this.activePartySlot.set(first);
    }
  }

  protected removeMember(index: number, event: MouseEvent): void {
    event.stopPropagation();
    const slots = [...this.partySlots()];
    slots[index] = null;
    this.partySlots.set(slots);
  }

  protected updateSlotColor(slot: number, color: string): void {
    const colors = [...this.slotColors()];
    colors[slot] = color;
    this.slotColors.set(colors);
  }

  protected getSlotPortrait(slot: PartyMember | null): string {
    if (!slot) return this.roleIconEmpty;
    const classList = this.classes();
    const cls = classList ? classList[slot.classIndex] : undefined;
    if (!cls) return Assets.Images.Characters.Placeholder;
    return `assets/images/characters/${cls.name.toLowerCase()}/${slot.gender}/portrait.webp`;
  }

  protected onStart(): void {
    if (this.isStarting() || !this.canStart()) return;

    const slots = this.partySlots();
    const colors = this.slotColors();
    const classList = this.classes();

    const partyMembers = slots
      .map((slot, i) => ({ slot, i }))
      .filter((item) => item.slot !== null)
      .map((item) => ({
        classId: classList[item.slot!.classIndex].id,
        color: colors[item.i],
        gender: item.slot!.gender === 'male' ? 0 : 1,
      }));

    const seed = this.gameSeed().trim() || null;

    this.isStarting.set(true);
    this.singlePlayerService
      .create({ seed, partyMembers })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (game) => {
          this.router.navigate(['/game', game.id]);
        },
        error: (err) => {
          console.error('Failed to create game', err);
          this.isStarting.set(false);
        },
      });
  }

  protected updateSeed(value: string): void {
    this.gameSeed.set(value);
  }

  protected statPercent(value: number, total: number): number {
    const percent = (value / total) * 100;
    return Math.min(Math.max(percent, 0), 100);
  }

  protected onBack(): void {
    this.router.navigate(['/']);
  }

  protected openDeckOverlay(): void {
    const character = this.selectedCharacter();
    if (!character) return;

    this.isDeckOverlayOpen.set(true);
    this.isDeckLoading.set(true);
    this.hasDeckError.set(false);
    this.deckCards.set([]);

    this.charactersService
      .getCharacterCards(character.id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (cards) => {
          this.deckCards.set(cards);
          this.isDeckLoading.set(false);
        },
        error: (err) => {
          console.error('Failed to load character cards', err);
          this.hasDeckError.set(true);
          this.isDeckLoading.set(false);
        },
      });
  }

  protected closeDeckOverlay(): void {
    this.isDeckOverlayOpen.set(false);
    this.hoveredCard.set(null);
  }

  protected hoverCard(card: CardDto): void {
    this.hoveredCard.set(card);
    this.tooltipLeft.set(-9999);
    this.tooltipTop.set(-9999);
    setTimeout(() => {
      this.adjustTooltipPosition();
    }, 0);
  }

  protected clearHoveredCard(): void {
    this.hoveredCard.set(null);
  }

  private adjustTooltipPosition(): void {
    const overlay = document.querySelector('.deck-overlay');
    const tooltip = document.querySelector('.card-preview-tooltip-overlay');
    const cardEl = document.querySelector('.deck-card-preview-display');

    if (!overlay || !tooltip || !cardEl) return;

    const overlayRect = overlay.getBoundingClientRect();
    const tooltipRect = tooltip.getBoundingClientRect();
    const cardRect = cardEl.getBoundingClientRect();

    const cardLeftInOverlay = cardRect.left - overlayRect.left;
    const cardRightInOverlay = cardRect.right - overlayRect.left;
    const cardTopInOverlay = cardRect.top - overlayRect.top;

    const gap = 16;
    const margin = 16;
    const tooltipWidth = tooltipRect.width || 260;
    const tooltipHeight = tooltipRect.height || 350;

    // Default left to the right of the card
    let leftPos = cardRightInOverlay + gap;

    // If it overflows right, place it to the left of the card
    if (leftPos + tooltipWidth > overlayRect.width - margin) {
      leftPos = cardLeftInOverlay - gap - tooltipWidth;
    }

    // Clamp leftPos
    if (leftPos < margin) {
      leftPos = margin;
    }

    // Default top aligned with the top of the card
    let topPos = cardTopInOverlay;

    // Adjust for bottom overflow
    const overflowBottom = (topPos + tooltipHeight) - (overlayRect.height - margin);
    if (overflowBottom > 0) {
      topPos -= overflowBottom;
    }

    // Clamp topPos
    if (topPos < margin) {
      topPos = margin;
    }

    this.tooltipLeft.set(leftPos);
    this.tooltipTop.set(topPos);
  }

  getClass(): number {
    return this.selectedCharacter()?.id ?? 0;
  }

  getGender(): string {
    return this.selectedGender();
  }
}
