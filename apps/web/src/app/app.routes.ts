import { Routes } from '@angular/router';

import { authGuard } from './core/auth/auth.guard';
import { CallbackComponent } from './features/callback/callback';
import { CharacterSelectionComponent } from './features/character-selection/character-selection';
import { EncounterComponent } from './features/encounter/encounter';
import { GameComponent } from './features/game/game';
import { HomeComponent } from './features/home/home';
import { LoginComponent } from './features/login/login';
import { GameContainerComponent } from './shared/game-container/game-container';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'callback', component: CallbackComponent },
  {
    path: '',
    component: GameContainerComponent,
    children: [
      { path: '', component: HomeComponent, canActivate: [authGuard] },
      { path: 'character-selection', component: CharacterSelectionComponent, canActivate: [authGuard] },
      {
        path: 'game/:gameId',
        component: GameComponent,
        canActivate: [authGuard],
        children: [
          { path: 'encounter', component: EncounterComponent },
        ],
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
