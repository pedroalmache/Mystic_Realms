import { Component, DestroyRef, OnInit, inject, input, output, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { Assets } from '../../core/constants/assets.constants';
import {
  NextRoomDirection,
  RewardType,
  RoomOptionDto,
  RoomOptionsDto,
  SinglePlayerService,
} from '../../core/services/single-player.service';

@Component({
  selector: 'app-room-selection',
  imports: [],
  templateUrl: './room-selection.html',
  styleUrl: './room-selection.scss',
})
export class RoomSelectionComponent implements OnInit {
  readonly gameId = input<number | null>(null);
  readonly roomSelected = output<NextRoomDirection>();

  private readonly singlePlayerService = inject(SinglePlayerService);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly background = Assets.Images.Backgrounds.RoomSelection;
  protected readonly roomIcon = Assets.Images.Icons.Room;
  protected readonly NextRoomDirection = NextRoomDirection;

  protected readonly options = signal<RoomOptionsDto | null>(null);
  protected readonly isLoading = signal(true);
  protected readonly hasError = signal(false);

  ngOnInit(): void {
    const id = this.gameId();
    if (!id) return;

    this.singlePlayerService
      .getRoomOptions(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (opts) => {
          this.options.set(opts);
          this.isLoading.set(false);
        },
        error: () => {
          this.hasError.set(true);
          this.isLoading.set(false);
        },
      });
  }

  protected selectRoom(direction: NextRoomDirection): void {
    this.roomSelected.emit(direction);
  }

  protected getOverlayIcon(option: RoomOptionDto): string {
    const key = (option.key ?? '').toLowerCase();

    if (key === 'treasure') return Assets.Images.Icons.Chest;
    if (key === 'gold') return Assets.Images.Icons.Pouch;
    if (key === 'merchant') return Assets.Images.Icons.Scale;
    if (key === 'smith') return Assets.Images.Icons.Anvil;
    if (key === 'healing') return Assets.Images.Icons.Potion;

    return this.getRewardTypeIcon(option.reward);
  }

  protected getRoomLabel(option: RoomOptionDto): string {
    const key = option.key;
    if (!key) return '';

    return key
      .replace(/[-_]+/g, ' ')
      .replace(/([a-z])([A-Z])/g, '$1 $2')
      .trim()
      .split(' ')
      .filter((p) => p.length > 0)
      .map((p) => p.charAt(0).toUpperCase() + p.slice(1).toLowerCase())
      .join(' ');
  }

  private getRewardTypeIcon(reward: RewardType): string {
    switch (reward) {
      case RewardType.Health: return Assets.Images.Blessings.Health.TypeIcon;
      case RewardType.Offensive: return Assets.Images.Blessings.Offensive.TypeIcon;
      case RewardType.Defensive: return Assets.Images.Blessings.Defensive.TypeIcon;
      case RewardType.Mana: return Assets.Images.Blessings.Mana.TypeIcon;
      case RewardType.Gold: return Assets.Images.Blessings.Gold.TypeIcon;
      case RewardType.Speed: return Assets.Images.Blessings.Speed.TypeIcon;
      case RewardType.Misc: return Assets.Images.Blessings.Misc.TypeIcon;
      default: return '';
    }
  }
}
