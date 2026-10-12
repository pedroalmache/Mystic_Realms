import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

export interface ServerVersion {
  version: string;
}

@Injectable({ providedIn: 'root' })
export class CommonService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.serverUrl;

  getServerVersion(): Observable<ServerVersion> {
    return this.http.get<ServerVersion>(`${this.baseUrl}/api/common/version`);
  }
}
