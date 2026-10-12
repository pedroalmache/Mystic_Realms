import { Component, OnInit, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DestroyRef } from '@angular/core';

import { CommonService } from '../../core/services/common.service';

@Component({
  selector: 'app-server-version',
  templateUrl: './server-version.html',
  styleUrl: './server-version.scss',
})
export class ServerVersionComponent implements OnInit {
  private readonly commonService = inject(CommonService);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly version = signal<string | null>(null);

  ngOnInit(): void {
    this.commonService
      .getServerVersion()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => this.version.set(res.version),
        error: () => this.version.set('unavailable'),
      });
  }
}
