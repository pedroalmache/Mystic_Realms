import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-callback',
  template: `
    <div class="callback-container">
      <div class="callback-loading">
        <span class="spinner" aria-label="Loading…"></span>
        <p>Redirecting&hellip;</p>
      </div>
    </div>
  `,
  styles: `
    :host {
      display: flex;
      height: 100dvh;
      align-items: center;
      justify-content: center;
      background: radial-gradient(ellipse at 50% 0%, #1a0a2e 0%, #0a0012 70%);
      color: rgba(200 180 255 / 0.8);
      font-family: inherit;
    }
    .callback-container { text-align: center; }
    .callback-loading { display: flex; flex-direction: column; align-items: center; gap: 1rem; }
    .spinner {
      display: inline-block; width: 40px; height: 40px;
      border: 3px solid rgba(180 126 255 / 0.3);
      border-top-color: #b47eff; border-radius: 50%;
      animation: spin 0.8s linear infinite;
    }
    @keyframes spin { to { transform: rotate(360deg); } }
  `,
  imports: [],
})
export class CallbackComponent implements OnInit {
  private readonly router = inject(Router);

  ngOnInit(): void {
    this.router.navigate(['/login']);
  }
}
