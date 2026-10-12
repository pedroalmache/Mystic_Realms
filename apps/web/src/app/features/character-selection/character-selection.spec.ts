import { TestBed, ComponentFixture } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { CharacterSelectionComponent } from './character-selection';
import { CharactersService } from '../../core/services/characters.service';
import { SinglePlayerService, CardDto, Spirit } from '../../core/services/single-player.service';
import { vi, describe, it, expect, beforeEach, afterEach } from 'vitest';

describe('CharacterSelectionComponent Tooltip Positioning', () => {
  let component: CharacterSelectionComponent;
  let fixture: ComponentFixture<CharacterSelectionComponent>;
  let mockRouter: any;
  let mockCharactersService: any;
  let mockSinglePlayerService: any;

  beforeEach(async () => {
    mockRouter = {
      navigate: vi.fn(),
    };

    mockCharactersService = {
      getCharacters: vi.fn().mockReturnValue(of([])),
      getCharacterCards: vi.fn().mockReturnValue(of([])),
    };

    mockSinglePlayerService = {
      create: vi.fn().mockReturnValue(of({ id: 'test-game-id' })),
    };

    await TestBed.configureTestingModule({
      imports: [CharacterSelectionComponent],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: CharactersService, useValue: mockCharactersService },
        { provide: SinglePlayerService, useValue: mockSinglePlayerService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CharacterSelectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    vi.restoreAllMocks();
  });

  it('should adjust tooltip position correctly on the right with no horizontal overflow', async () => {
    const mockOverlay = {
      getBoundingClientRect: () => ({
        top: 0,
        bottom: 900,
        left: 0,
        right: 1600,
        width: 1600,
        height: 900,
      }),
    };

    const mockCardEl = {
      getBoundingClientRect: () => ({
        top: 100,
        bottom: 600,
        left: 400,
        right: 784,
        width: 384,
        height: 500,
      }),
    };

    const mockTooltip = {
      getBoundingClientRect: () => ({
        top: 0,
        bottom: 350,
        left: 0,
        right: 260,
        width: 260,
        height: 350,
      }),
    };

    const querySelectorSpy = vi.spyOn(document, 'querySelector');
    querySelectorSpy.mockImplementation((selector: string) => {
      if (selector === '.deck-overlay') return mockOverlay as any;
      if (selector === '.deck-card-preview-display') return mockCardEl as any;
      if (selector === '.card-preview-tooltip-overlay') return mockTooltip as any;
      return null;
    });

    const mockCard: CardDto = {
      id: 1,
      playerId: 1,
      cardId: 101,
      instanceId: 'inst-1',
      partyMemberId: 1,
      classId: 1,
      color: 'red',
      name: 'Power Strike',
      description: 'Gain {{Strength_IMG}} strength.',
      rarity: 1,
      isAscended: false,
      useCount: 0,
      manaCost: 1,
      attack: 10,
      armor: 0,
      magicAttack: 0,
      magicArmor: 0,
      mana: 0,
      strength: 1,
      defense: 0,
      draw: 0,
      effectMultiplier: 1,
      heal: 0,
      poison: 0,
      lifeCost: 0,
      playerTargets: 1,
      enemyTargets: 1,
      permanent: false,
      momentum: 0,
      daggers: 0,
      spirit: Spirit.None,
    };

    component['hoverCard'](mockCard);

    await new Promise((resolve) => setTimeout(resolve, 0));

    // Default right side: cardRightInOverlay + gap = 784 + 16 = 800
    // Default top: cardTopInOverlay = 100
    expect(component['tooltipLeft']()).toBe(800);
    expect(component['tooltipTop']()).toBe(100);
  });

  it('should move tooltip to the left of the card when overflowing right edge', async () => {
    const mockOverlay = {
      getBoundingClientRect: () => ({
        top: 0,
        bottom: 900,
        left: 0,
        right: 1600,
        width: 1600,
        height: 900,
      }),
    };

    // Positioned on the right side of the screen
    const mockCardEl = {
      getBoundingClientRect: () => ({
        top: 100,
        bottom: 600,
        left: 1100,
        right: 1484,
        width: 384,
        height: 500,
      }),
    };

    const mockTooltip = {
      getBoundingClientRect: () => ({
        top: 0,
        bottom: 350,
        left: 0,
        right: 260,
        width: 260,
        height: 350,
      }),
    };

    const querySelectorSpy = vi.spyOn(document, 'querySelector');
    querySelectorSpy.mockImplementation((selector: string) => {
      if (selector === '.deck-overlay') return mockOverlay as any;
      if (selector === '.deck-card-preview-display') return mockCardEl as any;
      if (selector === '.card-preview-tooltip-overlay') return mockTooltip as any;
      return null;
    });

    const mockCard: CardDto = {
      id: 1,
      playerId: 1,
      cardId: 101,
      instanceId: 'inst-1',
      partyMemberId: 1,
      classId: 1,
      color: 'red',
      name: 'Power Strike',
      description: 'Gain {{Strength_IMG}} strength.',
      rarity: 1,
      isAscended: false,
      useCount: 0,
      manaCost: 1,
      attack: 10,
      armor: 0,
      magicAttack: 0,
      magicArmor: 0,
      mana: 0,
      strength: 1,
      defense: 0,
      draw: 0,
      effectMultiplier: 1,
      heal: 0,
      poison: 0,
      lifeCost: 0,
      playerTargets: 1,
      enemyTargets: 1,
      permanent: false,
      momentum: 0,
      daggers: 0,
      spirit: Spirit.None,
    };

    component['hoverCard'](mockCard);

    await new Promise((resolve) => setTimeout(resolve, 0));

    // Right overflow triggered (1484 + 16 + 260 = 1760 > 1584)
    // Left position: cardLeftInOverlay - gap - tooltipWidth = 1100 - 16 - 260 = 824
    // Top position: cardTopInOverlay = 100
    expect(component['tooltipLeft']()).toBe(824);
    expect(component['tooltipTop']()).toBe(100);
  });

  it('should push tooltip up and clamp when overflowing bottom of overlay', async () => {
    const mockOverlay = {
      getBoundingClientRect: () => ({
        top: 0,
        bottom: 900,
        left: 0,
        right: 1600,
        width: 1600,
        height: 900,
      }),
    };

    const mockCardEl = {
      getBoundingClientRect: () => ({
        top: 600,
        bottom: 1100,
        left: 400,
        right: 784,
        width: 384,
        height: 500,
      }),
    };

    const mockTooltip = {
      getBoundingClientRect: () => ({
        top: 0,
        bottom: 350,
        left: 0,
        right: 260,
        width: 260,
        height: 350,
      }),
    };

    const querySelectorSpy = vi.spyOn(document, 'querySelector');
    querySelectorSpy.mockImplementation((selector: string) => {
      if (selector === '.deck-overlay') return mockOverlay as any;
      if (selector === '.deck-card-preview-display') return mockCardEl as any;
      if (selector === '.card-preview-tooltip-overlay') return mockTooltip as any;
      return null;
    });

    const mockCard: CardDto = {
      id: 1,
      playerId: 1,
      cardId: 101,
      instanceId: 'inst-1',
      partyMemberId: 1,
      classId: 1,
      color: 'red',
      name: 'Power Strike',
      description: 'Gain {{Strength_IMG}} strength.',
      rarity: 1,
      isAscended: false,
      useCount: 0,
      manaCost: 1,
      attack: 10,
      armor: 0,
      magicAttack: 0,
      magicArmor: 0,
      mana: 0,
      strength: 1,
      defense: 0,
      draw: 0,
      effectMultiplier: 1,
      heal: 0,
      poison: 0,
      lifeCost: 0,
      playerTargets: 1,
      enemyTargets: 1,
      permanent: false,
      momentum: 0,
      daggers: 0,
      spirit: Spirit.None,
    };

    component['hoverCard'](mockCard);

    await new Promise((resolve) => setTimeout(resolve, 0));

    // Default right side: 800
    // Default top: 600
    // 600 + 350 = 950 > 900 - 16 = 884
    // overflowBottom = 950 - 884 = 66
    // topPos = 600 - 66 = 534
    expect(component['tooltipLeft']()).toBe(800);
    expect(component['tooltipTop']()).toBe(534);
  });
});
