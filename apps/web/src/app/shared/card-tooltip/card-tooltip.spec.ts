import { describe, it, expect } from 'vitest';
import { Injector, runInInjectionContext, signal } from '@angular/core';
import { CardTooltipComponent } from './card-tooltip';
import { CardDto, Spirit } from '../../core/services/single-player.service';

describe('CardTooltipComponent Pure Class Test', () => {
  it('should detect legend items from description and properties', () => {
    const mockCard: CardDto = {
      id: 1,
      playerId: 1,
      cardId: 101,
      instanceId: 'inst-1',
      partyMemberId: 1,
      classId: 1,
      color: 'red',
      name: 'Power Strike',
      description: 'Gain {{Strength_IMG}} strength and deal {{Damage_IMG}} damage.',
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

    const injector = Injector.create({ providers: [] });
    let component!: CardTooltipComponent;
    runInInjectionContext(injector, () => {
      component = new CardTooltipComponent();
    });

    // Overwrite the card input signal
    (component as any).card = signal(mockCard);

    // Retrieve computed legendItems
    const legendItems = component['legendItems']();
    expect(legendItems.length).toBeGreaterThan(0);

    // It should have Strength and Damage icons
    const hasStrength = legendItems.some((item) => item.label === 'Strength');
    const hasDamage = legendItems.some((item) => item.label === 'Physical Damage');
    const hasArmor = legendItems.some((item) => item.label === 'Armor');

    expect(hasStrength).toBe(true);
    expect(hasDamage).toBe(true);
    expect(hasArmor).toBe(false);
  });

  it('should return empty legend list for a card with no matched icons', () => {
    const mockCard: CardDto = {
      id: 2,
      playerId: 1,
      cardId: 102,
      instanceId: 'inst-2',
      partyMemberId: 1,
      classId: 1,
      color: 'blue',
      name: 'Blank Slate',
      description: 'Does absolutely nothing.',
      rarity: 1,
      isAscended: false,
      useCount: 0,
      manaCost: 0,
      attack: 0,
      armor: 0,
      magicAttack: 0,
      magicArmor: 0,
      mana: 0,
      strength: 0,
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

    const injector = Injector.create({ providers: [] });
    let component!: CardTooltipComponent;
    runInInjectionContext(injector, () => {
      component = new CardTooltipComponent();
    });

    // Overwrite the card input signal
    (component as any).card = signal(mockCard);

    const legendItems = component['legendItems']();
    expect(legendItems.length).toBe(0);
  });
});
