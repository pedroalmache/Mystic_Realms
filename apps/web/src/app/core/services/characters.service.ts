import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { CardDto } from './single-player.service';

export interface CharacterResult {
  id: number;
  name: string;
  description: string;
  hitPoints: number;
  speed: number;
  energy: number;
  isUnlocked: boolean;
}

@Injectable({ providedIn: 'root' })
export class CharactersService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.serverUrl}/api/Characters`;

  getCharacters(): Observable<CharacterResult[]> {
    return this.http.get<CharacterResult[]>(this.baseUrl);
  }

  getCharacterCards(characterId: number): Observable<CardDto[]> {
    return this.http.get<CardDto[]>(`${this.baseUrl}/${characterId}/cards`);
  }
}
