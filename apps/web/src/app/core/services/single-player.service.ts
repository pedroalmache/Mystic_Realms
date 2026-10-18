import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

export enum RoomType {
  Encounter = 0,
  EncounterReward = 1,
  Type2 = 2,
  Type3 = 3,
  Type4 = 4,
  Type5 = 5,
  Type6 = 6,
  Type7 = 7,
  Type8 = 8,
}

export enum RewardType {
  None = 0,
  Defensive = 1,
  Gold = 2,
  Health = 3,
  Mana = 4,
  Misc = 5,
  Offensive = 6,
  Speed = 7,
}

export enum Gender {
  Male = 0,
  Female = 1,
}

export enum GameActionType {
  SetInitial,
  UpdateTurn,
  UpdateInitiative,
  DiscardHand,
  Draw,
  ShuffleDeck,
  Mana,
  UpdateMana,
  CardPlayed,
  Discard,
  Armor,
  Attack,
  PhysicalDamage,
  MagicDamage,
  NotEnoughMana,
  Strength,
  Defense,
  EnemyDied,
  EnemyAttack,
  EnemyArmor,
  EnemyHealing,
  Healing,
  Died,
  RoomWon,
  GameOver,
  MagicArmor,
  MomentumAdd,
  MomentumRemove,
  Poison,
  PoisonConsumed,
  Daggers,
  Spirit,
  Speed,
  BoardAdd,
  Element,
}

export enum Spirit {
  None = 0,
  Wolf = 1,
  Bear = 2,
  Deer = 3,
  Owl = 4,
  Tortoise = 5,
}

export enum InitiativeType {
  Player = 0,
  Enemy = 1,
}

export interface ActiveSinglePlayerGameResult {
  gameId: number;
  code: string | null;
}

export interface GetSinglePlayerGameResult {
  gameId: number;
  level: number;
  path: string | null;
  actionLog: GameAction[] | null;
  room: RoomDto | null;
}

export interface GameDto {
  id: number;
  seed: string | null;
  playerCount: number;
}

export interface CreateSinglePlayerPartyMemberRequest {
  classId: number;
  color: string;
  gender: Gender;
}

export interface CreateSinglePlayerGameRequest {
  seed?: string | null;
  partyMembers: CreateSinglePlayerPartyMemberRequest[];
}

export enum NextRoomDirection {
  Left = 0,
  Right = 1,
  Forward = 2,
}

export interface NextRoomRequest {
  direction: NextRoomDirection;
}

export interface RoomDto {
  type: RoomType;
  reward: RewardType;
  key: string | null;
  backgroundId: number;
}

export interface RoomOptionDto {
  type: RoomType;
  reward: RewardType;
  key: string | null;
  backgroundId: number;
}

export interface RoomOptionsDto {
  left: RoomOptionDto;
  right: RoomOptionDto;
}

export interface CardDto {
  id: number;
  playerId: number;
  cardId: number;
  instanceId: string;
  partyMemberId: number;
  classId: number;
  color: string | null;
  name: string | null;
  description: string | null;
  rarity: number;
  isAscended: boolean;
  useCount: number;
  manaCost: number;
  attack: number;
  armor: number;
  magicAttack: number;
  magicArmor: number;
  mana: number;
  strength: number;
  defense: number;
  draw: number;
  effectMultiplier: number;
  heal: number;
  poison: number;
  lifeCost: number;
  playerTargets: number;
  enemyTargets: number;
  permanent: boolean;

  momentum: number;
  daggers: number;
  spirit: Spirit;

  fire?: boolean | null;
  water?: boolean | null;
  earth?: boolean | null;
  air?: boolean | null;
}

export interface PlayerGameAction {
  id: number;
  userId: string | null;
  hand: CardDto[] | null;
  mana: number;
}

export interface PartyMemberDto {
  partyMemberId: number;
  playerId: number;
  identifier: string;
  color: string | null;
  classId: number;
  gender: Gender;
  health: number;
  maxHealth: number;
  armor: number;
  magicArmor?: number;
  strength: number;
  defense: number;
  mana: number;
  speed: number;
  className: string | null;
  momentum?: number;
  poison?: number;
  daggers?: number;
  spirit?: Spirit;
  fire?: boolean | null;
  water?: boolean | null;
  earth?: boolean | null;
  air?: boolean | null;
}

export type EnemyIntentionType = 'Attack' | 'Defense' | 'Special';

export interface EnemyDto {
  instanceId: string;
  id: number;
  name: string | null;
  hitPoints: number;
  maxHitPoints: number;
  armor: number;
  strength: number;
  defense: number;
  speed: number;
  poison?: number;
  daggers?: number;
  intentions?: EnemyIntentionType[] | null;
}

export interface InitiativeDto {
  type: InitiativeType;
  enemyInstanceId: string | null;
  playerInstanceId: number | null;
  name: string | null;
  speed: number;
}

export interface BoardDto {
  card: CardDto;
  partyMemberId: number;
  entryPoint: number;
}

export interface GameAction {
  type: GameActionType;
  turn: number | null;
  players: PlayerGameAction[] | null;
  initiative: InitiativeDto[] | null;
  card: CardDto[] | null;
  partyMembers: PartyMemberDto[] | null;
  enemies: EnemyDto[] | null;
  room: RoomDto | null;
  board: BoardDto[] | null;
}

export interface GetDrawPileResult {
  cards: CardDto[] | null;
}

export interface GetDiscardPileResult {
  cards: CardDto[] | null;
}

export interface GetPlayerDeckResult {
  cards: CardDto[] | null;
}

export interface PlayCardRequest {
  cardInstanceId: string;
  partyMemberInstanceIds: string[] | null;
  enemyInstanceIds: string[] | null;
}

export interface BlessingDto {
  blessingId: number;
  type: number;
  name: string | null;
  rarity: number;
  description: string | null;
}

export interface RewardPlayerDto {
  playerId: number;
  userId: string | null;
  gamerTag: string | null;
  cards: CardDto[];
  partyMembers: RewardPartyMemberDto[];
}

export interface RewardPartyMemberDto {
  partyMemberId: number;
  identifier: string;
  color: string | null;
  classId: number;
  gender: Gender;
  health: number;
  maxHealth: number;
  mana: number;
  speed: number;
  className: string | null;
  gold: number;
}

export interface RoomRewardsDto {
  players: RewardPlayerDto[];
  blessings: BlessingDto[];
}

export interface ConfirmRewardItem {
  partyMemberId: number;
  cardId: number;
}

const SINGLE_PLAYER_BASE = `${environment.serverUrl}/api/SinglePlayer`;

@Injectable({ providedIn: 'root' })
export class SinglePlayerService {
  private readonly http = inject(HttpClient);

  /** Returns the current active single-player game, or null if none exists. */
  getActive(): Observable<ActiveSinglePlayerGameResult | null> {
    return this.http.get<ActiveSinglePlayerGameResult | null>(`${SINGLE_PLAYER_BASE}/active`);
  }

  /** Returns a single-player game by id. */
  getById(gameId: number): Observable<GetSinglePlayerGameResult> {
    return this.http.get<GetSinglePlayerGameResult>(`${SINGLE_PLAYER_BASE}/${gameId}`);
  }

  /** Creates a new single-player game. */
  create(request: CreateSinglePlayerGameRequest): Observable<GameDto> {
    return this.http.post<GameDto>(`${SINGLE_PLAYER_BASE}`, request);
  }

  /** Requests the next room for a game. */
  nextRoom(gameId: number, request: NextRoomRequest): Observable<GameAction[]> {
    return this.http.post<GameAction[]>(`${SINGLE_PLAYER_BASE}/${gameId}/next-room`, request);
  }

  /** Returns the action log for a game. */
  getActionLog(gameId: number): Observable<GameAction[]> {
    return this.http.get<GameAction[]>(`${SINGLE_PLAYER_BASE}/${gameId}/action-log`);
  }

  /** Returns the draw pile for a game. */
  getDrawPile(gameId: number): Observable<GetDrawPileResult> {
    return this.http.get<GetDrawPileResult>(`${SINGLE_PLAYER_BASE}/${gameId}/draw-pile`);
  }

  /** Returns the discard pile for a game. */
  getDiscardPile(gameId: number): Observable<GetDiscardPileResult> {
    return this.http.get<GetDiscardPileResult>(`${SINGLE_PLAYER_BASE}/${gameId}/discard-pile`);
  }

  /** Returns a player's deck for a game. */
  getPlayerDeck(gameId: number, playerId: number): Observable<GetPlayerDeckResult> {
    return this.http.get<GetPlayerDeckResult>(
      `${SINGLE_PLAYER_BASE}/${gameId}/players/${playerId}/deck`,
    );
  }

  /** Advances to the next turn for a game. */
  nextTurn(gameId: number): Observable<GameAction[]> {
    return this.http.post<GameAction[]>(`${SINGLE_PLAYER_BASE}/${gameId}/next-turn`, {});
  }

  /** Plays a card for a game. */
  playCard(gameId: number, request: PlayCardRequest): Observable<GameAction[]> {
    return this.http.post<GameAction[]>(`${SINGLE_PLAYER_BASE}/${gameId}/play-card`, request);
  }

  /** Returns the room rewards for a game. */
  getRoomRewards(gameId: number): Observable<RoomRewardsDto> {
    return this.http.get<RoomRewardsDto>(`${SINGLE_PLAYER_BASE}/${gameId}/room-rewards`);
  }

  /** Confirms the selected room reward for a game. */
  confirmReward(gameId: number, payload: ConfirmRewardItem[]): Observable<void> {
    return this.http.post<void>(`${SINGLE_PLAYER_BASE}/${gameId}/confirm-reward`, payload);
  }

  /** Returns the room options (left/right choices) for a game. */
  getRoomOptions(gameId: number): Observable<RoomOptionsDto> {
    return this.http.get<RoomOptionsDto>(`${SINGLE_PLAYER_BASE}/${gameId}/room-options`);
  }
}
