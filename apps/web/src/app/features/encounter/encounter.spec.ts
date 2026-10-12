import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EncounterComponent } from './encounter';
import { SinglePlayerService, InitiativeType } from '../../core/services/single-player.service';
import { of } from 'rxjs';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { vi } from 'vitest';

describe('EncounterComponent', () => {
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

  it('should display speed in the initiative bar', () => {
    // Set up initiative entries
    const initiative = [
      {
        type: InitiativeType.Player,
        enemyInstanceId: null,
        playerInstanceId: 1,
        name: 'Hero',
        speed: 15
      }
    ];

    // Trigger processSetInitial via action log input
    fixture.componentRef.setInput('actionLog', [
      {
        type: 0, // SetInitial
        initiative: initiative,
        players: [{ id: 1, userId: 'test-user', hand: [], mana: 3 }],
        partyMembers: [],
        enemies: [],
        turn: 1
      }
    ]);

    fixture.detectChanges();

    const speedValue = fixture.nativeElement.querySelector('.speed-value');
    const speedIcon = fixture.nativeElement.querySelector('.speed-icon');

    expect(speedValue).toBeTruthy();
    expect(speedValue.textContent).toContain('15');
    expect(speedIcon).toBeTruthy();
    expect(speedIcon.getAttribute('src')).toContain('speed.png');
  });
});
