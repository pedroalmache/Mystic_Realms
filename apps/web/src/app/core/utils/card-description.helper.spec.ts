import { describe, it, expect } from 'vitest';
import { CardDescriptionHelper } from './card-description.helper';

describe('CardDescriptionHelper', () => {
  const defaultValues = {
    attack: 10,
    armor: 5,
    magicAttack: 8,
    magicArmor: 4,
    healing: 12,
    poison: 3,
    mana: 2,
    draw: 1,
    strength: 2,
    defense: 3,
    effectMultiplier: 1,
  };

  describe('parseDescription', () => {
    it('should replace stat placeholders with values', () => {
      const description = 'Deal {{Attack}} damage, heal {{Heal}} and gain {{Armor}} armor.';
      const result = CardDescriptionHelper.parseDescription(description, defaultValues);

      expect(result).toEqual([
        { type: 'text', value: 'Deal 10 damage, heal 12 and gain 5 armor.' },
      ]);
    });

    it('should handle multiple placeholders of the same type', () => {
      const description = '{{Attack}} and {{Attack}}';
      const result = CardDescriptionHelper.parseDescription(description, defaultValues);
      expect(result).toEqual([{ type: 'text', value: '10 and 10' }]);
    });

    it('should identify image tokens', () => {
      const description = 'Gain {{Mana}} {{Mana_IMG}} and {{Heal_IMG}} with {{Speed_IMG}}';
      const result = CardDescriptionHelper.parseDescription(description, defaultValues);

      expect(result).toEqual([
        { type: 'text', value: 'Gain 2 ' },
        { type: 'image', value: 'assets/images/icons/mana.png' },
        { type: 'text', value: ' and ' },
        { type: 'image', value: 'assets/images/icons/hp.png' },
        { type: 'text', value: ' with ' },
        { type: 'image', value: 'assets/images/icons/speed.png' },
        { type: 'text', value: '' },
      ]);
    });

    it('should identify dagger token', () => {
      const description = '{{Dagger}}';
      const result = CardDescriptionHelper.parseDescription(description, defaultValues);

      expect(result).toEqual([
        { type: 'text', value: '' },
        { type: 'image', value: 'assets/images/characters/assassin/powers/poison.png' },
        { type: 'text', value: '' },
      ]);
    });

    it('should support {{NL}} for new lines', () => {
      const description = 'Line 1{{NL}}Line 2';
      const result = CardDescriptionHelper.parseDescription(description, defaultValues);

      expect(result).toContainEqual({ type: 'newline', value: '' });
    });

    it('should identify elemental tokens', () => {
      const description = '{{Fire}} {{Water}} {{Earth}} {{Air}}';
      const result = CardDescriptionHelper.parseDescription(description, defaultValues);

      expect(result).toEqual([
        { type: 'text', value: '' },
        { type: 'image', value: 'assets/images/characters/elementalist/powers/fire.png' },
        { type: 'text', value: ' ' },
        { type: 'image', value: 'assets/images/characters/elementalist/powers/water.png' },
        { type: 'text', value: ' ' },
        { type: 'image', value: 'assets/images/characters/elementalist/powers/earth.png' },
        { type: 'text', value: ' ' },
        { type: 'image', value: 'assets/images/characters/elementalist/powers/air.png' },
        { type: 'text', value: '' },
      ]);
    });

    it('should identify throwing knife token', () => {
      const description = '{{Throwing_Knife}}';
      const result = CardDescriptionHelper.parseDescription(description, defaultValues);

      expect(result).toEqual([
        { type: 'text', value: '' },
        { type: 'image', value: 'assets/images/characters/assassin/powers/knife2.png' },
        { type: 'text', value: '' },
      ]);
    });

    it('should identify momentum token', () => {
      const description = '{{Momentum}}';
      const result = CardDescriptionHelper.parseDescription(description, defaultValues);

      expect(result).toEqual([
        { type: 'text', value: '' },
        { type: 'image', value: 'assets/images/characters/warrior/powers/momentum.png' },
        { type: 'text', value: '' },
      ]);
    });
  });
});
