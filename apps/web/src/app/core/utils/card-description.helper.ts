import { Assets } from '../constants/assets.constants';
import { Spirit } from '../services/single-player.service';

export interface DescriptionPart {
  type: 'text' | 'image' | 'newline';
  value: string;
}

export class CardDescriptionHelper {
  static parseDescription(
    description: string,
    values: {
      attack: number;
      armor: number;
      magicAttack: number;
      magicArmor: number;
      healing: number;
      poison: number;
      mana: number;
      draw: number;
      strength: number;
      defense: number;
      effectMultiplier: number;
    },
  ): DescriptionPart[] {
    let display = description;
    display = display.replaceAll('{{Attack}}', values.attack.toString());
    display = display.replaceAll('{{Armor}}', values.armor.toString());
    display = display.replaceAll('{{MagicAttack}}', values.magicAttack.toString());
    display = display.replaceAll('{{MagicArmor}}', values.magicArmor.toString());
    display = display.replaceAll('{{Healing}}', values.healing.toString());
    display = display.replaceAll('{{Heal}}', values.healing.toString());
    display = display.replaceAll('{{Poison}}', values.poison.toString());
    display = display.replaceAll('{{Mana}}', values.mana.toString());
    display = display.replaceAll('{{Draw}}', values.draw.toString());
    display = display.replaceAll('{{Strength}}', values.strength.toString());
    display = display.replaceAll('{{Defense}}', values.defense.toString());
    display = display.replaceAll('{{EffectMultiplier}}', values.effectMultiplier.toString());

    return display
      .split(
        /({{Poison_IMG}}|{{Armor_IMG}}|{{Damage_IMG}}|{{Heal_IMG}}|{{Mana_IMG}}|{{Card_IMG}}|{{Strength_IMG}}|{{Defense_IMG}}|{{Magic_Damage_IMG}}|{{Magic_Armor_IMG}}|{{Speed_IMG}}|{{Bear_Spirit}}|{{Wolf_Spirit}}|{{Deer_Spirit}}|{{Owl_Spirit}}|{{Tortoise_Spirit}}|{{Throwing_Knife}}|{{Dagger}}|{{Fire}}|{{Water}}|{{Earth}}|{{Air}}|{{Momentum}}|{{NL}})/g,
      )
      .map((part): DescriptionPart => {
        if (part === '{{NL}}') return { type: 'newline', value: '' };
        if (part === '{{Poison_IMG}}') return { type: 'image', value: Assets.Images.Icons.Poison };
        if (part === '{{Armor_IMG}}') return { type: 'image', value: Assets.Images.Icons.Shield };
        if (part === '{{Damage_IMG}}') return { type: 'image', value: Assets.Images.Icons.Damage };
        if (part === '{{Heal_IMG}}') return { type: 'image', value: Assets.Images.Icons.Hp };
        if (part === '{{Magic_Damage_IMG}}')
          return { type: 'image', value: Assets.Images.Icons.Magic };
        if (part === '{{Magic_Armor_IMG}}')
          return { type: 'image', value: Assets.Images.Icons.MagicShield };
        if (part === '{{Mana_IMG}}') return { type: 'image', value: Assets.Images.Icons.Mana };
        if (part === '{{Card_IMG}}')
          return { type: 'image', value: Assets.Images.Cards.Display.CardBackSmall };
        if (part === '{{Strength_IMG}}')
          return { type: 'image', value: Assets.Images.Icons.StrengthTiny };
        if (part === '{{Defense_IMG}}')
          return { type: 'image', value: Assets.Images.Icons.ArmorTiny };
        if (part === '{{Speed_IMG}}') return { type: 'image', value: Assets.Images.Icons.Speed };
        if (part === '{{Bear_Spirit}}')
          return { type: 'image', value: Assets.Images.Characters.Druid.Powers.Bear };
        if (part === '{{Wolf_Spirit}}')
          return { type: 'image', value: Assets.Images.Characters.Druid.Powers.Wolf };
        if (part === '{{Deer_Spirit}}')
          return { type: 'image', value: Assets.Images.Characters.Druid.Powers.Deer };
        if (part === '{{Owl_Spirit}}')
          return { type: 'image', value: Assets.Images.Characters.Druid.Powers.Owl };
        if (part === '{{Tortoise_Spirit}}')
          return { type: 'image', value: Assets.Images.Characters.Druid.Powers.Tortoise };
        if (part === '{{Fire}}')
          return { type: 'image', value: Assets.Images.Characters.Elementalist.Powers.Fire };
        if (part === '{{Water}}')
          return { type: 'image', value: Assets.Images.Characters.Elementalist.Powers.Water };
        if (part === '{{Earth}}')
          return { type: 'image', value: Assets.Images.Characters.Elementalist.Powers.Earth };
        if (part === '{{Air}}')
          return { type: 'image', value: Assets.Images.Characters.Elementalist.Powers.Air };
        if (part === '{{Throwing_Knife}}')
          return { type: 'image', value: Assets.Images.Characters.Assassin.Powers.ThrowingKnife };
        if (part === '{{Dagger}}')
          return { type: 'image', value: Assets.Images.Characters.Assassin.Powers.Daggers };
        if (part === '{{Momentum}}')
          return { type: 'image', value: Assets.Images.Characters.Warrior.Powers.Momentum };
        return { type: 'text', value: part };
      });
  }

  static resolveRole(characterId: number): string {
    switch (characterId) {
      case 1:
        return 'warrior';
      case 2:
        return 'assassin';
      case 3:
        return 'ranger';
      case 4:
        return 'paladin';
      case 5:
        return 'white mage';
      case 6:
        return 'black mage';
      case 7:
        return 'elementalist';
      case 8:
        return 'druid';
      case 9:
        return 'barbarian';
      default:
        return '';
    }
  }

  static getSpiritIcon(spirit: Spirit): string {
    switch (spirit) {
      case Spirit.Wolf:
        return Assets.Images.Characters.Druid.Powers.Wolf;
      case Spirit.Bear:
        return Assets.Images.Characters.Druid.Powers.Bear;
      case Spirit.Deer:
        return Assets.Images.Characters.Druid.Powers.Deer;
      case Spirit.Owl:
        return Assets.Images.Characters.Druid.Powers.Owl;
      case Spirit.Tortoise:
        return Assets.Images.Characters.Druid.Powers.Tortoise;
      default:
        return '';
    }
  }
}
