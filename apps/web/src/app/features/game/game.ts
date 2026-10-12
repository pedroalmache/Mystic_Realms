import { Component, DestroyRef, OnDestroy, OnInit, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable, map, of, switchMap, tap } from 'rxjs';

import {
  GameAction,
  GameActionType,
  GetSinglePlayerGameResult,
  NextRoomDirection,
  RoomType,
  SinglePlayerService,
} from '../../core/services/single-player.service';
import { EncounterComponent } from '../encounter/encounter';
import { RoomRewardsComponent } from '../room-rewards/room-rewards';
import { RoomSelectionComponent } from '../room-selection/room-selection';

@Component({
  selector: 'app-game',
  templateUrl: './game.html',
  styleUrl: './game.scss',
  imports: [RouterOutlet, EncounterComponent, RoomRewardsComponent, RoomSelectionComponent],
})
export class GameComponent implements OnInit, OnDestroy {
  private static readonly ROOM_SWITCH_FADE_MS = 280;

  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly singlePlayerService = inject(SinglePlayerService);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly isLoading = signal(true);
  protected readonly hasError = signal(false);
  protected readonly game = signal<GetSinglePlayerGameResult | null>(null);
  protected readonly actionLog = signal<GameAction[]>([]);
  protected readonly isEncounter = computed(() => this.game()?.room?.type === RoomType.Encounter);
  private readonly showEncounterRewards = signal(false);
  protected readonly isRoomRewards = computed(
    () => this.showEncounterRewards() || this.game()?.room?.type === RoomType.EncounterReward,
  );
  protected readonly isRoomSelection = computed(
    () => this.game()?.room != null && !this.isEncounter() && !this.isRoomRewards(),
  );
  protected readonly isRoomSwitching = signal(false);

  private roomSwitchTimeout: ReturnType<typeof setTimeout> | null = null;

  ngOnInit(): void {
    const gameId = Number(this.route.snapshot.paramMap.get('gameId'));

    if (!gameId || isNaN(gameId)) {
      this.router.navigate(['/']);
      return;
    }

    this.loadGame(gameId);
  }

  ngOnDestroy(): void {
    if (this.roomSwitchTimeout !== null) {
      clearTimeout(this.roomSwitchTimeout);
      this.roomSwitchTimeout = null;
    }
  }

  protected onEndTurn(): void {
    const gameId = this.game()?.gameId;
    if (gameId === undefined) return;

    this.singlePlayerService
      .nextTurn(gameId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (actions) => {
          this.appendActions(actions);
        },
        error: (err) => {
          console.error('Failed to advance turn', err);
        },
      });
  }

  protected onActionsReceived(actions: GameAction[]): void {
    this.appendActions(actions);
  }

  protected onRoomWon(): void {
    this.showEncounterRewards.set(true);
  }

  protected onRoomSelected(direction: NextRoomDirection): void {
    const gameId = this.game()?.gameId;
    if (!gameId || this.isRoomSwitching()) return;

    this.isRoomSwitching.set(true);
    this.singlePlayerService
      .nextRoom(gameId, { direction })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.afterFade(GameComponent.ROOM_SWITCH_FADE_MS).then(() => {
            this.loadGame(gameId, true, () => {
              this.isRoomSwitching.set(false);
            });
          });
        },
        error: (err) => {
          console.error('Failed to select room', err);
          this.isRoomSwitching.set(false);
        },
      });
  }

  protected onRewardConfirmed(): void {
    const gameId = this.game()?.gameId;
    if (!gameId || this.isRoomSwitching()) return;

    this.isRoomSwitching.set(true);
    this.afterFade(GameComponent.ROOM_SWITCH_FADE_MS).then(() => {
      this.showEncounterRewards.set(false);
      this.loadGame(gameId, true, () => {
        this.isRoomSwitching.set(false);
      });
    });
  }

  private loadGame(gameId: number, replaceActionLog = false, onDone?: () => void): void {
    this.getGameLoad$(gameId, replaceActionLog)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.isLoading.set(false);
          onDone?.();
        },
        error: (err) => {
          console.error('Failed to load game', err);
          this.hasError.set(true);
          this.isLoading.set(false);
          onDone?.();
        },
      });
  }

  private getGameLoad$(gameId: number, replaceActionLog: boolean): Observable<void> {
    return this.singlePlayerService
      .getById(gameId)
      .pipe(
        tap((game) => {
          this.game.set(game);
        }),
        switchMap((game) => {
          const existingLog = game.actionLog ?? [];
          if (existingLog.length > 0) {
            if (replaceActionLog) {
              this.actionLog.set(existingLog);
            } else {
              this.appendActions(existingLog);
            }
            return of(void 0);
          }
          if (game.room?.type === RoomType.Encounter) {
            return this.singlePlayerService.nextTurn(gameId).pipe(
              tap((actions) => {
                if (replaceActionLog) {
                  this.actionLog.set(actions);
                } else {
                  this.appendActions(actions);
                }
              }),
              map(() => void 0),
            );
          }
          if (replaceActionLog) {
            this.actionLog.set([]);
          }
          return of(void 0);
        }),
      );
  }

  private afterFade(ms: number): Promise<void> {
    return new Promise((resolve) => {
      this.roomSwitchTimeout = setTimeout(() => {
        this.roomSwitchTimeout = null;
        resolve();
      }, ms);
    });
  }

  private appendActions(actions: GameAction[]): void {
    this.actionLog.update((log) => [...log, ...actions]);
    if (actions.some((action) => action.type === GameActionType.SetInitial)) {
      this.showEncounterRewards.set(false);
    }
  }
}
