import { Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, switchMap } from 'rxjs';

import { Assets } from '../../core/constants/assets.constants';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl: './login.scss',
  imports: [FormsModule],
})
export class LoginComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  protected readonly isLoading = signal(false);
  protected readonly errorMessage = signal<string | null>(null);
  protected readonly isSignUp = signal(false);

  protected readonly logoDragon = Assets.Images.Logos.LogoDragon;

  protected email = '';
  protected password = '';
  protected gamerTag = '';

  private readonly submit$ = new Subject<void>();

  constructor() {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/']);
    }

    this.submit$
      .pipe(
        switchMap(() => {
          return this.isSignUp()
            ? this.authService.signUp(this.email, this.password, this.gamerTag)
            : this.authService.login(this.email, this.password);
        }),
        takeUntilDestroyed(),
      )
      .subscribe({
        next: () => this.router.navigate(['/']),
        error: () => {
          this.errorMessage.set(
            this.isSignUp()
              ? 'Sign-up failed. Please check your details and try again.'
              : 'Login failed. Please check your credentials and try again.',
          );
          this.isLoading.set(false);
        },
      });
  }

  protected toggleMode(): void {
    this.isSignUp.update((v) => !v);
    this.errorMessage.set(null);
  }

  protected onSubmit(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.submit$.next();
  }
}
