import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take, tap } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

const AUTH_BASE = `${environment.serverUrl}/api/Auth`;

/** Auth endpoints that must not receive a Bearer token (prevents infinite loops). */
const AUTH_ENDPOINTS = new Set([
  `${AUTH_BASE}/login`,
  `${AUTH_BASE}/signup`,
  `${AUTH_BASE}/refresh`,
]);

function addAuthHeader(req: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
  return req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
}

function handle401(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: AuthService,
): Observable<HttpEvent<unknown>> {
  // Only one concurrent refresh should be in-flight at a time.
  if (!authService._isRefreshing) {
    authService._isRefreshing = true;
    authService._refreshTokenSubject.next(null);

    return authService.refreshToken().pipe(
      tap((tokens) => {
        authService._isRefreshing = false;
        authService._refreshTokenSubject.next(tokens.accessToken);
      }),
      switchMap((tokens) => next(addAuthHeader(req, tokens.accessToken ?? ''))),
      catchError((err) => {
        authService._isRefreshing = false;
        // refreshToken() already calls clearTokens; navigate to /login.
        authService.logout();
        return throwError(() => err);
      }),
    );
  }

  // Another request already triggered the refresh – wait for the new token.
  return authService._refreshTokenSubject.pipe(
    filter((token): token is string => token !== null),
    take(1),
    switchMap((token) => next(addAuthHeader(req, token))),
  );
}

/**
 * HTTP interceptor that:
 *  1. Attaches the Bearer JWT access token to every outgoing request.
 *  2. On a 401 response, transparently refreshes the token and retries.
 *  3. If the refresh fails (or there is no refresh token), logs the user out.
 *
 * Requests to the auth endpoints themselves are left untouched to prevent an
 * infinite refresh loop.
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  // Skip token injection for auth endpoints.
  if (AUTH_ENDPOINTS.has(req.url)) {
    return next(req);
  }

  const token = authService.getAccessToken();
  const authReq = token ? addAuthHeader(req, token) : req;

  return next(authReq).pipe(
    catchError((error) => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        return handle401(req, next, authService);
      }
      return throwError(() => error);
    }),
  );
};
