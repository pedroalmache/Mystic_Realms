import { Component, computed, input } from '@angular/core';
import { Assets } from '../../core/constants/assets.constants';
import { CardDto } from '../../core/services/single-player.service';
import { CardDescriptionHelper, DescriptionPart } from '../../core/utils/card-description.helper';

export interface LegendItem {
  icon: string;
  label: string;
  meaning: string;
}

@Component({
  selector: 'app-card-tooltip',
  standalone: true,
  templateUrl: './card-tooltip.html',
  styleUrl: './card-tooltip.scss',
})
export class CardTooltipComponent {
  readonly card = input.required<CardDto>();

  protected readonly Assets = Assets;

  protected readonly parsedDescription = computed<DescriptionPart[]>(() => {
    const c = this.card();
    return CardDescriptionHelper.parseDescription(c.description ?? '', {
      attack: c.attack,
      armor: c.armor,
      magicAttack: c.magicAttack,
      magicArmor: c.magicArmor,
      healing: c.heal,
      poison: c.poison,
      mana: c.mana,
      draw: c.draw,
      strength: c.strength,
      defense: c.defense,
      effectMultiplier: c.effectMultiplier,
    });
  });

  protected readonly legendItems = computed<LegendItem[]>(() => {
    const c = this.card();
    const desc = c.description ?? '';
    const items: LegendItem[] = [];

    const definitions: {
      check: (card: CardDto, description: string) => boolean;
      icon: string;
      label: string;
      meaning: string;
    }[] = [
      {
        check: (card, d) =>
          d.includes('{{Strength_IMG}}') || (card.strength !== undefined && card.strength > 0),
        icon: Assets.Images.Icons.StrengthTiny,
        label: 'Strength',
        meaning: 'Adds bonus damage',
      },
      {
        check: (card, d) =>
          d.includes('{{Defense_IMG}}') || (card.defense !== undefined && card.defense > 0),
        icon: Assets.Images.Icons.ArmorTiny,
        label: 'Defense',
        meaning: 'Adds bonus armor',
      },
      {
        check: (card, d) =>
          d.includes('{{Armor_IMG}}') || (card.armor !== undefined && card.armor > 0),
        icon: Assets.Images.Icons.Shield,
        label: 'Armor',
        meaning: 'Reduces physical damage taken',
      },
      {
        check: (card, d) =>
          d.includes('{{Magic_Armor_IMG}}') ||
          (card.magicArmor !== undefined && card.magicArmor > 0),
        icon: Assets.Images.Icons.MagicShield,
        label: 'Magic Armor',
        meaning: 'Reduces magic damage taken',
      },
      {
        check: (card, d) =>
          d.includes('{{Damage_IMG}}') || (card.attack !== undefined && card.attack > 0),
        icon: Assets.Images.Icons.Damage,
        label: 'Physical Damage',
        meaning: 'Deals physical damage',
      },
      {
        check: (card, d) =>
          d.includes('{{Magic_Damage_IMG}}') ||
          (card.magicAttack !== undefined && card.magicAttack > 0),
        icon: Assets.Images.Icons.Magic,
        label: 'Magic Damage',
        meaning: 'Deals magical damage',
      },
      {
        check: (card, d) =>
          d.includes('{{Heal_IMG}}') || (card.heal !== undefined && card.heal > 0),
        icon: Assets.Images.Icons.Hp,
        label: 'Healing',
        meaning: 'Restores Health',
      },
      {
        check: (card, d) =>
          d.includes('{{Mana_IMG}}') || (card.mana !== undefined && card.mana > 0),
        icon: Assets.Images.Icons.Mana,
        label: 'Mana',
        meaning: 'Needed to play cards',
      },
      {
        check: (card, d) =>
          d.includes('{{Card_IMG}}') || (card.draw !== undefined && card.draw > 0),
        icon: Assets.Images.Cards.Display.CardBackSmall,
        label: 'Card',
        meaning: 'Playable Cards',
      },
      {
        check: (card, d) => d.includes('{{Speed_IMG}}'),
        icon: Assets.Images.Icons.Speed,
        label: 'Speed',
        meaning: 'Determines turn order',
      },
      {
        check: (card, d) =>
          d.includes('{{Poison_IMG}}') || (card.poison !== undefined && card.poison > 0),
        icon: Assets.Images.Icons.Poison,
        label: 'Poison',
        meaning: 'Inflicts damage over time',
      },
      {
        check: (card, d) =>
          d.includes('{{Momentum}}'),
        icon: Assets.Images.Characters.Warrior.Powers.Momentum,
        label: 'Momentum',
        meaning: 'Use 4 to share armor with party',
      },
      {
        check: (card, d) =>
          d.includes('{{Dagger}}'),
        icon: Assets.Images.Characters.Assassin.Powers.Daggers,
        label: 'Dagger',
        meaning: 'Apply poison when it reaches 0',
      },
      {
        check: (card, d) => d.includes('{{Throwing_Knife}}'),
        icon: Assets.Images.Characters.Assassin.Powers.ThrowingKnife,
        label: 'Throwing Knife',
        meaning: 'Thorwing Knife Card',
      },
      {
        check: (card, d) => d.includes('{{Bear_Spirit}}') || card.spirit === 2,
        icon: Assets.Images.Characters.Druid.Powers.Bear,
        label: 'Bear Spirit',
        meaning: 'Druid form',
      },
      {
        check: (card, d) => d.includes('{{Wolf_Spirit}}') || card.spirit === 1,
        icon: Assets.Images.Characters.Druid.Powers.Wolf,
        label: 'Wolf Spirit',
        meaning: 'Druid form',
      },
      {
        check: (card, d) => d.includes('{{Deer_Spirit}}') || card.spirit === 3,
        icon: Assets.Images.Characters.Druid.Powers.Deer,
        label: 'Deer Spirit',
        meaning: 'Druid form',
      },
      {
        check: (card, d) => d.includes('{{Owl_Spirit}}') || card.spirit === 4,
        icon: Assets.Images.Characters.Druid.Powers.Owl,
        label: 'Owl Spirit',
        meaning: 'Druid form',
      },
      {
        check: (card, d) => d.includes('{{Tortoise_Spirit}}') || card.spirit === 5,
        icon: Assets.Images.Characters.Druid.Powers.Tortoise,
        label: 'Tortoise Spirit',
        meaning: 'Druid form',
      },
      {
        check: (card, d) => d.includes('{{Fire}}') || card.fire === true,
        icon: Assets.Images.Characters.Elementalist.Powers.Fire,
        label: 'Fire',
        meaning: 'Elemental essence',
      },
      {
        check: (card, d) => d.includes('{{Water}}') || card.water === true,
        icon: Assets.Images.Characters.Elementalist.Powers.Water,
        label: 'Water',
        meaning: 'Elemental essence',
      },
      {
        check: (card, d) => d.includes('{{Earth}}') || card.earth === true,
        icon: Assets.Images.Characters.Elementalist.Powers.Earth,
        label: 'Earth',
        meaning: 'Elemental essence',
      },
      {
        check: (card, d) => d.includes('{{Air}}') || card.air === true,
        icon: Assets.Images.Characters.Elementalist.Powers.Air,
        label: 'Air',
        meaning: 'Elemental essence',
      },
    ];

    for (const def of definitions) {
      if (def.check(c, desc)) {
        items.push({
          icon: def.icon,
          label: def.label,
          meaning: def.meaning,
        });
      }
    }

    return items;
  });
}
