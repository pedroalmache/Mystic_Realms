import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EncounterComponent } from './encounter';
import { SinglePlayerService, InitiativeType, GameActionType } from '../../core/services/single-player.service';
import { of } from 'rxjs';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { vi } from 'vitest';

describe('EncounterComponent Board', () => {
  let component: EncounterComponent;
  let fixture: ComponentFixture<EncounterComponent>;
  let mockSinglePlayerService: any;
  let mockRouter: any;

  beforeEach(async () => {
    mockSinglePlayerService = {
      getDrawPile: vi.fn().mockReturnValue(of({ cards: [] })),
      getDiscardPile: vi.fn().mockReturnValue(of({ cards: [] })),
      playCard: vi.fn().mockReturnValue(of([]))
    };
    mockRouter = {
      navigate: vi.fn()
    };

    await TestBed.configureTestingModule({
      imports: [EncounterComponent],
      providers: [
        { provide: SinglePlayerService, useValue: mockSinglePlayerService },
        { provide: Router, useValue: mockRouter },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(EncounterComponent);
    component = fixture.componentInstance;
  });

  it('should initialize the board from SetInitial action', async () => {
    const board = [
      {
        card: {
          id: 1,
          playerId: 1,
          cardId: 1,
          instanceId: 'card-1',
          partyMemberId: 1,
          classId: 1,
          name: 'Strike',
          description: 'Deal {{Attack}} damage',
          attack: 10
        } as any,
        partyMemberId: 1,
        entryPoint: 0
      }
    ];

    fixture.componentRef.setInput('actionLog', [
      {
        type: GameActionType.SetInitial,
        board: board,
        players: [{ id: 1, userId: 'test-user', hand: [], mana: 3 }],
        turn: 1
      }
    ]);

    await vi.waitFor(() => {
      fixture.detectChanges();
      const boardCards = fixture.nativeElement.querySelectorAll('.board-card');
      expect(boardCards.length).toBe(1);
    });

    const img = fixture.nativeElement.querySelector('.board-card img');
    expect(img.getAttribute('src')).toContain('assets/images/cards/warrior/1.webp');
  });

  it('should update the board from BoardAdd action', async () => {
    const initialBoard = [
        {
          card: {
            id: 1,
            cardId: 1,
            instanceId: 'card-1',
            classId: 1,
            name: 'Strike',
          } as any,
          partyMemberId: 1,
          entryPoint: 0
        }
      ];

    const updatedBoard = [
        ...initialBoard,
        {
          card: {
            id: 2,
            cardId: 2,
            instanceId: 'card-2',
            classId: 1,
            name: 'Defend',
          } as any,
          partyMemberId: 1,
          entryPoint: 0
        }
    ];

    fixture.componentRef.setInput('actionLog', [
      {
        type: GameActionType.SetInitial,
        board: initialBoard,
        players: [{ id: 1, userId: 'test-user', hand: [], mana: 3 }],
        partyMembers: [],
        enemies: [],
        initiative: [],
        turn: 1,
      },
    ]);

    fixture.detectChanges();
    await fixture.whenStable();

    fixture.componentRef.setInput('actionLog', [
      {
        type: GameActionType.SetInitial,
        board: initialBoard,
        players: [{ id: 1, userId: 'test-user', hand: [], mana: 3 }],
        partyMembers: [],
        enemies: [],
        initiative: [],
        turn: 1,
      },
      {
        type: GameActionType.BoardAdd,
        board: updatedBoard,
      },
    ]);

    await vi.waitFor(() => {
      fixture.detectChanges();
      const boardCards = fixture.nativeElement.querySelectorAll('.board-card');
      if (boardCards.length !== 2) {
          throw new Error(`Expected 2 cards, got ${boardCards.length}`);
      }
    }, { timeout: 2000 });
  });
});
