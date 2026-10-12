import {
  Component,
  DestroyRef,
  computed,
  effect,
  inject,
  input,
  output,
  signal,
  untracked,
} from '@angular/core';

import {
  HEAL_ANIMATION_MS,
  POISON_ANIMATION_MS,
  MOMENTUM_REMOVE_FADE_MS,
  MOMENTUM_REMOVE_STAGGER_MS,
  ELEMENT_ANIMATION_MS,
} from '../../core/constants/animation.constants';
import { Assets } from '../../core/constants/assets.constants';
import { Spirit } from '../../core/services/single-player.service';

@Component({
  selector: 'app-game-character',
  templateUrl: './game-character.html',
  styleUrl: './game-character.scss',
})
export class GameCharacterComponent {
  private static readonly ARMOR_BURST_MS = 1400;
  private static readonly STRENGTH_BURST_MS = 1400;
  private static readonly ATTACK_ANIMATION_MS = 600;
  private static readonly DEFENSE_ANIMATION_MS = 600;
  private static readonly HP_DELTA_ANIMATION_MS = 1500;
  private readonly destroyRef = inject(DestroyRef);

  readonly spriteUrl = input.required<string>();
  readonly altText = input<string>('character');
  readonly currentHp = input<number>(0);
  readonly maxHp = input<number>(0);
  readonly armor = input<number>(0);
  readonly magicArmor = input<number>(0);
  readonly armorDelta = input<number | null>(null);
  readonly armorAnimationKey = input<number>(0);
  readonly armorType = input<'physical' | 'magic'>('physical');
  readonly strength = input<number>(0);
  readonly defense = input<number>(0);
  readonly strengthDelta = input<number | null>(null);
  readonly strengthAnimationKey = input<number>(0);
  readonly defenseDelta = input<number | null>(null);
  readonly defenseAnimationKey = input<number>(0);
  readonly speed = input<number>(0);
  readonly speedDelta = input<number | null>(null);
  readonly speedAnimationKey = input<number>(0);
  readonly attackingSprite = input<string | null>(null);
  readonly attackAnimationKey = input<number>(0);
  readonly defendingSprite = input<string | null>(null);
  readonly defendAnimationKey = input<number>(0);
  readonly hpDelta = input<number | null>(null);
  readonly hpDeltaTone = input<'default' | 'healing'>('default');
  readonly healingSprite = input<string | null>(null);
  readonly healAnimationKey = input<number>(0);
  readonly poison = input<number>(0);
  readonly poisonSprite = input<string | null>(null);
  readonly poisonAnimationKey = input<number>(0);
  readonly poisonDelta = input<number | null>(null);
  readonly className = input<string | null>(null);
  readonly nameColor = input<string | null>(null);
  readonly isSelectable = input<boolean>(false);
  readonly isSelected = input<boolean>(false);
  readonly selectedColor = input<string | null>(null);
  readonly momentum = input<number>(0);
  readonly daggers = input<number>(0);
  readonly daggersAnimationKey = input<number>(0);
  readonly spirit = input<number>(0);
  readonly spiritAnimationKey = input<number>(0);
  readonly elementAnimationKey = input<number>(0);
  readonly fire = input<boolean>(false);
  readonly water = input<boolean>(false);
  readonly earth = input<boolean>(false);
  readonly air = input<boolean>(false);

  readonly characterClicked = output<void>();

  protected readonly Assets = Assets;
  private readonly displayedMomentumCount = signal<number>(0);
  protected readonly isDaggersFadingOut = signal<boolean>(false);
  protected readonly isSpiritFadingOut = signal<boolean>(false);
  protected readonly isFireFadingOut = signal<boolean>(false);
  protected readonly isWaterFadingOut = signal<boolean>(false);
  protected readonly isEarthFadingOut = signal<boolean>(false);
  protected readonly isAirFadingOut = signal<boolean>(false);
  protected readonly isSpiriting = signal<boolean>(false);
  private readonly fadingMomentumIds = signal<Set<number>>(new Set());
  private readonly pendingMomentumRemovals = signal<number>(0);
  private momentumStaggerTimeout: ReturnType<typeof setTimeout> | null = null;

  protected readonly momentumRows = computed(() => {
    const activeCount = this.displayedMomentumCount();
    const fadingIds = this.fadingMomentumIds();

    const maxId = Math.max(activeCount - 1, ...(fadingIds.size > 0 ? Array.from(fadingIds) : [-1]));

    if (maxId < 0) return [];

    const rows: Array<{ id: number; items: Array<{ id: number; fading: boolean }> }> = [];
    let currentTotal = 0;
    let rowIndex = 0;

    while (currentTotal <= maxId) {
      const countInRow = Math.min(4, maxId + 1 - currentTotal);
      const rowItems: Array<{ id: number; fading: boolean }> = [];
      for (let i = 0; i < countInRow; i++) {
        const id = currentTotal + i;
        rowItems.push({ id, fading: fadingIds.has(id) });
      }
      rows.push({ id: rowIndex, items: rowItems });
      currentTotal += countInRow;
      rowIndex++;
    }

    return rows;
  });
  protected readonly showArmorBurst = signal<boolean>(false);
  protected readonly displayedArmorDelta = signal<number | null>(null);
  protected readonly showStrengthBurst = signal<boolean>(false);
  protected readonly displayedStrengthDelta = signal<number | null>(null);
  protected readonly showDefenseBurst = signal<boolean>(false);
  protected readonly displayedDefenseDelta = signal<number | null>(null);
  protected readonly showSpeedBurst = signal<boolean>(false);
  protected readonly displayedSpeedDelta = signal<number | null>(null);
  protected readonly isAttacking = signal<boolean>(false);
  protected readonly isDefending = signal<boolean>(false);
  protected readonly isHealing = signal<boolean>(false);
  protected readonly showHpDelta = signal<boolean>(false);
  protected readonly displayedHpDelta = signal<number | null>(null);

  protected readonly isPoisoning = signal<boolean>(false);
  protected readonly showPoisonDelta = signal<boolean>(false);
  protected readonly displayedPoisonDelta = signal<number | null>(null);

  protected readonly displayedSprite = computed(() => {
    if (this.isAttacking() && this.attackingSprite()) return this.attackingSprite()!;
    if (this.isDefending() && this.defendingSprite()) return this.defendingSprite()!;
    return this.spriteUrl();
  });
  protected readonly armorIcon = computed(() =>
    this.armorType() === 'magic' ? Assets.Images.Icons.MagicShield : Assets.Images.Icons.Shield,
  );

  private armorBurstTimeout: ReturnType<typeof setTimeout> | null = null;
  private strengthBurstTimeout: ReturnType<typeof setTimeout> | null = null;
  private defenseBurstTimeout: ReturnType<typeof setTimeout> | null = null;
  private speedBurstTimeout: ReturnType<typeof setTimeout> | null = null;
  private attackAnimationTimeout: ReturnType<typeof setTimeout> | null = null;
  private defendAnimationTimeout: ReturnType<typeof setTimeout> | null = null;
  private healAnimationTimeout: ReturnType<typeof setTimeout> | null = null;
  private hpDeltaTimeout: ReturnType<typeof setTimeout> | null = null;
  private poisonAnimationTimeout: ReturnType<typeof setTimeout> | null = null;
  private poisonDeltaTimeout: ReturnType<typeof setTimeout> | null = null;
  private daggersFadeTimeout: ReturnType<typeof setTimeout> | null = null;
  private spiritFadeTimeout: ReturnType<typeof setTimeout> | null = null;
  private spiritAnimationTimeout: ReturnType<typeof setTimeout> | null = null;
  private fireFadeTimeout: ReturnType<typeof setTimeout> | null = null;
  private waterFadeTimeout: ReturnType<typeof setTimeout> | null = null;
  private earthFadeTimeout: ReturnType<typeof setTimeout> | null = null;
  private airFadeTimeout: ReturnType<typeof setTimeout> | null = null;
  private isDestroyed = false;

  private prevFire = false;
  private prevWater = false;
  private prevEarth = false;
  private prevAir = false;

  protected readonly spiritIcon = computed(() => {
    switch (this.spirit()) {
      case Spirit.Wolf:
        return Assets.Images.Characters.Druid.Powers.Wolf;
      case Spirit.Bear:
        return Assets.Images.Characters.Druid.Powers.Bear;
      case Spirit.Deer:
        return Assets.Images.Characters.Druid.Powers.Deer;
      case Spirit.Owl:
        return Assets.Images.Characters.Druid.Powers.Owl;
      case Spirit.Tortoise:
        return Assets.Images.Characters.Druid.Powers.Tortoise;
      default:
        return null;
    }
  });

  protected readonly displayedSpiritIcon = signal<string | null>(null);

  constructor() {
    effect(() => {
      const icon = this.spiritIcon();
      if (icon !== null) {
        untracked(() => this.displayedSpiritIcon.set(icon));
      }
    });

    effect(
      () => {
        const target = this.momentum();
        untracked(() => {
          const currentCount = this.displayedMomentumCount();
          const pending = this.pendingMomentumRemovals();

          if (target > currentCount + pending) {
            this.displayedMomentumCount.set(target);
            this.pendingMomentumRemovals.set(0);
            this.fadingMomentumIds.set(new Set());
            if (this.momentumStaggerTimeout !== null) {
              clearTimeout(this.momentumStaggerTimeout);
              this.momentumStaggerTimeout = null;
            }
          } else if (target < currentCount - pending) {
            const additionalToRemove = currentCount - pending - target;
            this.pendingMomentumRemovals.set(pending + additionalToRemove);
            if (this.momentumStaggerTimeout === null) {
              this.processNextMomentumRemoval();
            }
          }
        });
      },
      { allowSignalWrites: true },
    );

    effect(() => {
      const animationKey = this.armorAnimationKey();
      const delta = this.armorDelta();
      if (animationKey <= 0 || delta == null || delta <= 0) {
        return;
      }

      if (this.armorBurstTimeout !== null) {
        clearTimeout(this.armorBurstTimeout);
        this.armorBurstTimeout = null;
      }

      this.displayedArmorDelta.set(delta);
      this.showArmorBurst.set(false);

      queueMicrotask(() => {
        if (this.isDestroyed) {
          return;
        }

        this.showArmorBurst.set(true);
        this.armorBurstTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.showArmorBurst.set(false);
          }
          this.armorBurstTimeout = null;
        }, GameCharacterComponent.ARMOR_BURST_MS);
      });
    });

    effect(() => {
      const animationKey = this.strengthAnimationKey();
      const delta = this.strengthDelta();
      if (animationKey <= 0 || delta == null || !Number.isFinite(delta) || delta === 0) {
        return;
      }

      if (this.strengthBurstTimeout !== null) {
        clearTimeout(this.strengthBurstTimeout);
        this.strengthBurstTimeout = null;
      }

      this.displayedStrengthDelta.set(delta);
      this.showStrengthBurst.set(false);

      queueMicrotask(() => {
        if (this.isDestroyed) {
          return;
        }

        this.showStrengthBurst.set(true);
        this.strengthBurstTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.showStrengthBurst.set(false);
          }
          this.strengthBurstTimeout = null;
        }, GameCharacterComponent.STRENGTH_BURST_MS);
      });
    });

    effect(() => {
      const animationKey = this.speedAnimationKey();
      const delta = this.speedDelta();
      if (animationKey <= 0 || delta == null || !Number.isFinite(delta) || delta === 0) {
        return;
      }

      if (this.speedBurstTimeout !== null) {
        clearTimeout(this.speedBurstTimeout);
        this.speedBurstTimeout = null;
      }

      this.displayedSpeedDelta.set(delta);
      this.showSpeedBurst.set(false);

      queueMicrotask(() => {
        if (this.isDestroyed) {
          return;
        }

        this.showSpeedBurst.set(true);
        this.speedBurstTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.showSpeedBurst.set(false);
          }
          this.speedBurstTimeout = null;
        }, GameCharacterComponent.STRENGTH_BURST_MS);
      });
    });

    effect(() => {
      const animationKey = this.defenseAnimationKey();
      const delta = this.defenseDelta();
      if (animationKey <= 0 || delta == null || !Number.isFinite(delta) || delta === 0) {
        return;
      }

      if (this.defenseBurstTimeout !== null) {
        clearTimeout(this.defenseBurstTimeout);
        this.defenseBurstTimeout = null;
      }

      this.displayedDefenseDelta.set(delta);
      this.showDefenseBurst.set(false);

      queueMicrotask(() => {
        if (this.isDestroyed) {
          return;
        }

        this.showDefenseBurst.set(true);
        this.defenseBurstTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.showDefenseBurst.set(false);
          }
          this.defenseBurstTimeout = null;
        }, GameCharacterComponent.STRENGTH_BURST_MS);
      });
    });

    effect(() => {
      const animationKey = this.attackAnimationKey();
      if (animationKey <= 0) {
        return;
      }

      if (this.attackAnimationTimeout !== null) {
        clearTimeout(this.attackAnimationTimeout);
        this.attackAnimationTimeout = null;
      }

      this.isAttacking.set(false);

      queueMicrotask(() => {
        if (this.isDestroyed) {
          return;
        }

        this.isAttacking.set(true);
        this.attackAnimationTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.isAttacking.set(false);
          }
          this.attackAnimationTimeout = null;
        }, GameCharacterComponent.ATTACK_ANIMATION_MS);
      });
    });

    effect(() => {
      const animationKey = this.defendAnimationKey();
      const delta = this.hpDelta();
      if (animationKey <= 0) {
        return;
      }

      if (this.defendAnimationTimeout !== null) {
        clearTimeout(this.defendAnimationTimeout);
        this.defendAnimationTimeout = null;
      }
      if (this.hpDeltaTimeout !== null) {
        clearTimeout(this.hpDeltaTimeout);
        this.hpDeltaTimeout = null;
      }

      this.isDefending.set(false);
      this.showHpDelta.set(false);

      queueMicrotask(() => {
        if (this.isDestroyed) {
          return;
        }

        this.isDefending.set(true);
        this.defendAnimationTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.isDefending.set(false);
          }
          this.defendAnimationTimeout = null;
        }, GameCharacterComponent.DEFENSE_ANIMATION_MS);

        if (delta != null && delta !== 0) {
          this.displayedHpDelta.set(delta);
          this.showHpDelta.set(true);
          this.hpDeltaTimeout = setTimeout(() => {
            if (!this.isDestroyed) {
              this.showHpDelta.set(false);
            }
            this.hpDeltaTimeout = null;
          }, GameCharacterComponent.HP_DELTA_ANIMATION_MS);
        }
      });
    });

    effect(() => {
      const animationKey = this.healAnimationKey();
      const delta = this.hpDelta();
      if (animationKey <= 0) {
        return;
      }

      if (this.healAnimationTimeout !== null) {
        clearTimeout(this.healAnimationTimeout);
        this.healAnimationTimeout = null;
      }
      if (this.hpDeltaTimeout !== null) {
        clearTimeout(this.hpDeltaTimeout);
        this.hpDeltaTimeout = null;
      }

      this.isHealing.set(false);
      this.showHpDelta.set(false);

      queueMicrotask(() => {
        if (this.isDestroyed) {
          return;
        }

        this.isHealing.set(true);
        this.healAnimationTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.isHealing.set(false);
          }
          this.healAnimationTimeout = null;
        }, HEAL_ANIMATION_MS);

        if (delta != null && delta !== 0) {
          this.displayedHpDelta.set(delta);
          this.showHpDelta.set(true);
          this.hpDeltaTimeout = setTimeout(() => {
            if (!this.isDestroyed) {
              this.showHpDelta.set(false);
            }
            this.hpDeltaTimeout = null;
          }, HEAL_ANIMATION_MS);
        }
      });
    });

    effect(() => {
      const animationKey = this.poisonAnimationKey();
      const delta = this.poisonDelta();
      if (animationKey <= 0) {
        return;
      }

      if (this.poisonAnimationTimeout !== null) {
        clearTimeout(this.poisonAnimationTimeout);
        this.poisonAnimationTimeout = null;
      }
      if (this.poisonDeltaTimeout !== null) {
        clearTimeout(this.poisonDeltaTimeout);
        this.poisonDeltaTimeout = null;
      }

      this.isPoisoning.set(false);
      this.showPoisonDelta.set(false);

      queueMicrotask(() => {
        if (this.isDestroyed) {
          return;
        }

        this.isPoisoning.set(true);
        this.poisonAnimationTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.isPoisoning.set(false);
          }
          this.poisonAnimationTimeout = null;
        }, POISON_ANIMATION_MS);

        if (delta != null && delta !== 0) {
          this.displayedPoisonDelta.set(delta);
          this.showPoisonDelta.set(true);
          this.poisonDeltaTimeout = setTimeout(() => {
            if (!this.isDestroyed) {
              this.showPoisonDelta.set(false);
            }
            this.poisonDeltaTimeout = null;
          }, POISON_ANIMATION_MS);
        }
      });
    });

    effect(() => {
      const key = this.daggersAnimationKey();
      if (key <= 0) return;

      if (this.daggers() === 0) {
        if (this.daggersFadeTimeout !== null) {
          clearTimeout(this.daggersFadeTimeout);
        }
        this.isDaggersFadingOut.set(true);
        this.daggersFadeTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.isDaggersFadingOut.set(false);
          }
          this.daggersFadeTimeout = null;
        }, 600); // 0.6s to match keyframes
      } else {
        // If it got daggers again while fading, clear the fade
        this.isDaggersFadingOut.set(false);
        if (this.daggersFadeTimeout !== null) {
          clearTimeout(this.daggersFadeTimeout);
          this.daggersFadeTimeout = null;
        }
      }
    });

    effect(() => {
      const fire = this.fire();
      const water = this.water();
      const earth = this.earth();
      const air = this.air();
      const key = this.elementAnimationKey();

      untracked(() => {
        if (key > 0) {
          if (this.prevFire && !fire) {
            this.startElementFade('fire');
          }
          if (this.prevWater && !water) {
            this.startElementFade('water');
          }
          if (this.prevEarth && !earth) {
            this.startElementFade('earth');
          }
          if (this.prevAir && !air) {
            this.startElementFade('air');
          }
        }
        this.prevFire = fire;
        this.prevWater = water;
        this.prevEarth = earth;
        this.prevAir = air;
      });
    });

    effect(() => {
      const key = this.spiritAnimationKey();
      if (key <= 0) return;

      if (this.spiritAnimationTimeout !== null) {
        clearTimeout(this.spiritAnimationTimeout);
      }

      this.isSpiriting.set(false);

      queueMicrotask(() => {
        if (this.isDestroyed) {
          return;
        }

        this.isSpiriting.set(true);
        this.spiritAnimationTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.isSpiriting.set(false);
          }
          this.spiritAnimationTimeout = null;
        }, 600);
      });

      if (this.spirit() === 0) {
        if (this.spiritFadeTimeout !== null) {
          clearTimeout(this.spiritFadeTimeout);
        }
        this.isSpiritFadingOut.set(true);
        this.spiritFadeTimeout = setTimeout(() => {
          if (!this.isDestroyed) {
            this.isSpiritFadingOut.set(false);
          }
          this.spiritFadeTimeout = null;
        }, 600); // 0.6s to match keyframes
      } else {
        // If it got spirit again while fading, clear the fade
        this.isSpiritFadingOut.set(false);
        if (this.spiritFadeTimeout !== null) {
          clearTimeout(this.spiritFadeTimeout);
          this.spiritFadeTimeout = null;
        }
      }
    });

    this.destroyRef.onDestroy(() => {
      this.isDestroyed = true;
      if (this.daggersFadeTimeout !== null) {
        clearTimeout(this.daggersFadeTimeout);
      }
      if (this.spiritFadeTimeout !== null) {
        clearTimeout(this.spiritFadeTimeout);
      }
      if (this.spiritAnimationTimeout !== null) {
        clearTimeout(this.spiritAnimationTimeout);
      }
      if (this.momentumStaggerTimeout !== null) {
        clearTimeout(this.momentumStaggerTimeout);
      }
      if (this.armorBurstTimeout !== null) {
        clearTimeout(this.armorBurstTimeout);
      }
      if (this.strengthBurstTimeout !== null) {
        clearTimeout(this.strengthBurstTimeout);
      }
      if (this.defenseBurstTimeout !== null) {
        clearTimeout(this.defenseBurstTimeout);
      }
      if (this.speedBurstTimeout !== null) {
        clearTimeout(this.speedBurstTimeout);
      }
      if (this.attackAnimationTimeout !== null) {
        clearTimeout(this.attackAnimationTimeout);
      }
      if (this.defendAnimationTimeout !== null) {
        clearTimeout(this.defendAnimationTimeout);
      }
      if (this.healAnimationTimeout !== null) {
        clearTimeout(this.healAnimationTimeout);
      }
      if (this.hpDeltaTimeout !== null) {
        clearTimeout(this.hpDeltaTimeout);
      }
      if (this.poisonAnimationTimeout !== null) {
        clearTimeout(this.poisonAnimationTimeout);
      }
      if (this.poisonDeltaTimeout !== null) {
        clearTimeout(this.poisonDeltaTimeout);
      }
      if (this.fireFadeTimeout !== null) {
        clearTimeout(this.fireFadeTimeout);
      }
      if (this.waterFadeTimeout !== null) {
        clearTimeout(this.waterFadeTimeout);
      }
      if (this.earthFadeTimeout !== null) {
        clearTimeout(this.earthFadeTimeout);
      }
      if (this.airFadeTimeout !== null) {
        clearTimeout(this.airFadeTimeout);
      }
    });
  }

  private startElementFade(element: 'fire' | 'water' | 'earth' | 'air'): void {
    const fadingSignal = {
      fire: this.isFireFadingOut,
      water: this.isWaterFadingOut,
      earth: this.isEarthFadingOut,
      air: this.isAirFadingOut,
    }[element];

    const timeoutKey = {
      fire: 'fireFadeTimeout',
      water: 'waterFadeTimeout',
      earth: 'earthFadeTimeout',
      air: 'airFadeTimeout',
    }[element] as 'fireFadeTimeout' | 'waterFadeTimeout' | 'earthFadeTimeout' | 'airFadeTimeout';

    if (this[timeoutKey] !== null) {
      clearTimeout(this[timeoutKey]!);
    }

    fadingSignal.set(true);
    this[timeoutKey] = setTimeout(() => {
      if (!this.isDestroyed) {
        fadingSignal.set(false);
      }
      this[timeoutKey] = null;
    }, ELEMENT_ANIMATION_MS);
  }

  protected readonly hpPercent = computed(() => {
    const max = this.maxHp();
    if (!max) return 0;
    return Math.min(100, Math.max(0, (this.currentHp() / max) * 100));
  });

  private processNextMomentumRemoval() {
    const pending = this.pendingMomentumRemovals();
    if (pending <= 0) return;

    const currentCount = this.displayedMomentumCount();
    if (currentCount <= 0) {
      this.pendingMomentumRemovals.set(0);
      return;
    }

    const targetIdToRemove = currentCount - 1;
    this.displayedMomentumCount.set(currentCount - 1);

    this.fadingMomentumIds.update((set) => {
      const next = new Set(set);
      next.add(targetIdToRemove);
      return next;
    });

    this.pendingMomentumRemovals.set(pending - 1);

    setTimeout(() => {
      if (this.isDestroyed) return;
      this.fadingMomentumIds.update((set) => {
        const next = new Set(set);
        next.delete(targetIdToRemove);
        return next;
      });
    }, MOMENTUM_REMOVE_FADE_MS);

    if (this.pendingMomentumRemovals() > 0) {
      this.momentumStaggerTimeout = setTimeout(() => {
        if (this.isDestroyed) return;
        this.momentumStaggerTimeout = null;
        this.processNextMomentumRemoval();
      }, MOMENTUM_REMOVE_STAGGER_MS);
    } else {
      this.momentumStaggerTimeout = null;
    }
  }

  protected armorDeltaText(): string {
    const delta = this.displayedArmorDelta();
    if (delta == null) return '';
    return `${delta > 0 ? '+' : ''}${delta}`;
  }

  protected strengthDeltaText(): string {
    const delta = this.displayedStrengthDelta();
    if (delta == null || !Number.isFinite(delta)) return '';
    return `${delta > 0 ? '+' : ''}${delta}`;
  }

  protected defenseDeltaText(): string {
    const delta = this.displayedDefenseDelta();
    if (delta == null || !Number.isFinite(delta)) return '';
    return `${delta > 0 ? '+' : ''}${delta}`;
  }

  protected onClick(): void {
    if (this.isSelectable()) {
      this.characterClicked.emit();
    }
  }
}
