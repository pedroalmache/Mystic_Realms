import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { environment } from '../../../environments/environment';

export interface AuthResponse {
  accessToken: string | null;
  refreshToken: string | null;
  expiresAt: string;
  gamerTag: string | null;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface SignUpRequest {
  email: string;
  password: string;
  gamerTag: string;
}

export interface RefreshTokenRequest {
  accessToken: string;
  refreshToken: string;
}

const ACCESS_TOKEN_KEY = 'mr_access_token';
const REFRESH_TOKEN_KEY = 'mr_refresh_token';

const AUTH_BASE = `${environment.serverUrl}/api/Auth`;

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly _isAuthenticated = signal(this.hasValidToken());
  readonly isAuthenticated = this._isAuthenticated.asReadonly();

  /** Used by the interceptor to queue concurrent requests while a refresh is in-flight. */
  _isRefreshing = false;
  readonly _refreshTokenSubject = new BehaviorSubject<string | null>(null);

  // ---------------------------------------------------------------------------
  // Public API
  // ---------------------------------------------------------------------------

  /** Authenticates the user with email and password. */
  login(email: string, password: string): Observable<AuthResponse> {
    const body: LoginRequest = { email, password };
    return this.http
      .post<AuthResponse>(`${AUTH_BASE}/login`, body)
      .pipe(tap((tokens) => this.storeTokens(tokens)));
  }

  /** Registers a new user account. */
  signUp(email: string, password: string, gamerTag: string): Observable<AuthResponse> {
    const body: SignUpRequest = { email, password, gamerTag };
    return this.http
      .post<AuthResponse>(`${AUTH_BASE}/signup`, body)
      .pipe(tap((tokens) => this.storeTokens(tokens)));
  }

  /** Exchanges the stored refresh token for a new token pair. */
  refreshToken(): Observable<AuthResponse> {
    const accessToken = localStorage.getItem(ACCESS_TOKEN_KEY);
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);

    if (!accessToken || !refreshToken) {
      return throwError(() => new Error('No tokens available for refresh'));
    }

    const body: RefreshTokenRequest = { accessToken, refreshToken };

    return this.http
      .post<AuthResponse>(`${AUTH_BASE}/refresh`, body)
      .pipe(
        tap((tokens) => this.storeTokens(tokens)),
        catchError((err) => {
          this.clearTokens();
          return throwError(() => err);
        }),
      );
  }

  /** Clears all stored tokens and navigates to the login page. */
  logout(): void {
    this.clearTokens();
    this.router.navigate(['/login']);
  }

  /** Returns the current access token from storage, or null if none exists. */
  getAccessToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
  }

  /** Returns the authenticated user's id claim from the current access token. */
  getCurrentUserId(): string | null {
    const token = this.getAccessToken();
    if (!token) {
      return null;
    }

    const parts = token.split('.');
    if (parts.length < 2) {
      return null;
    }

    try {
      // UI-only claim extraction: token authenticity/authorization is enforced by the API.
      const payload = JSON.parse(atob(parts[1])) as Record<string, unknown>;
      const userId =
        payload['sub'] ??
        payload['nameid'] ??
        payload['nameidentifier'] ??
        payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

      return typeof userId === 'string' ? userId : null;
    } catch (err) {
      console.warn('[AuthService] Failed to parse user id from access token.', err);
      return null;
    }
  }

  // ---------------------------------------------------------------------------
  // Internal helpers
  // ---------------------------------------------------------------------------

  private storeTokens(tokens: AuthResponse): void {
    if (tokens.accessToken) {
      localStorage.setItem(ACCESS_TOKEN_KEY, tokens.accessToken);
    }
    if (tokens.refreshToken) {
      localStorage.setItem(REFRESH_TOKEN_KEY, tokens.refreshToken);
    }
    this._isAuthenticated.set(true);
  }

  private clearTokens(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    this._isAuthenticated.set(false);
  }

  private hasValidToken(): boolean {
    return !!localStorage.getItem(ACCESS_TOKEN_KEY);
  }
}
