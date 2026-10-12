import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

import { App } from './app';
import { OAUTH_CONFIG } from './core/auth/auth.config';
import { routes } from './app.routes';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        provideRouter(routes),
        provideHttpClient(),
        {
          provide: OAUTH_CONFIG,
          useValue: {
            clientId: 'test-client',
            authorizationEndpoint: 'https://auth.example.com/oauth2/authorize',
            tokenEndpoint: 'https://auth.example.com/oauth2/token',
            redirectUri: 'http://localhost/callback',
            scopes: ['openid'],
          },
        },
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
