import { Component, OnInit, inject, signal } from '@angular/core';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';

import { AuthService } from '../../core/auth/auth.service';
import { Assets } from '../../core/constants/assets.constants';
import { ActiveSinglePlayerGameResult, SinglePlayerService } from '../../core/services/single-player.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  styleUrl: './home.scss',
  imports: [NgIf],
})
export class HomeComponent implements OnInit {
  protected readonly authService = inject(AuthService);
  private readonly singlePlayerService = inject(SinglePlayerService);
  private readonly router = inject(Router);

  protected readonly background = Assets.Images.Backgrounds.Home;
  protected readonly greenButton = Assets.Images.UI.GreenButton;
  protected readonly topTitleBg = Assets.Images.UI.TopTitleBg;
  protected readonly singlePlayerIcon = Assets.Images.UI.SinglePlayer;
  protected readonly multiPlayerIcon = Assets.Images.UI.MultiPlayer;

  protected readonly isLoading = signal(true);
  protected readonly activeGame = signal<ActiveSinglePlayerGameResult | null>(null);

  ngOnInit(): void {
    this.singlePlayerService.getActive().subscribe({
      next: (game) => {
        this.activeGame.set(game);
        this.isLoading.set(false);
      },
      error: () => {
        this.activeGame.set(null);
        this.isLoading.set(false);
      },
    });
  }

  protected onContinue(): void {
    const game = this.activeGame();
    if (game) {
      this.router.navigate(['/game', game.gameId]);
    }
  }

  protected onNewGame(): void {
    this.router.navigate(['/character-selection']);
  }

  protected onLogout(): void {
    this.authService.logout();
  }

  protected copyCode(): void {
    const code = this.activeGame()?.code;
    if (code) {
      navigator.clipboard.writeText(code);
    }
  }
}
