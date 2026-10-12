import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { ServerVersionComponent } from './shared/server-version/server-version';
import { DevToolsComponent } from './shared/dev-tools/dev-tools';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ServerVersionComponent, DevToolsComponent],
  template: `
    <router-outlet />
    <app-server-version />
    <app-dev-tools />
  `,
  styles: ':host { display: block; }',
})
export class App {}
