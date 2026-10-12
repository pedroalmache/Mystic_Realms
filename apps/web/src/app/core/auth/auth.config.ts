import { InjectionToken } from '@angular/core';

export interface OAuthConfig {
  clientId: string;
  authorizationEndpoint: string;
  tokenEndpoint: string;
  redirectUri: string;
  scopes: string[];
  /** Optional: full URL to hit on logout (e.g. the provider's end_session_endpoint). */
  logoutEndpoint?: string;
}

export const OAUTH_CONFIG = new InjectionToken<OAuthConfig>('OAUTH_CONFIG');
