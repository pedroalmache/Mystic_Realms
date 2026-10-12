import { Component, computed, input } from '@angular/core';
import { Assets } from '../../core/constants/assets.constants';
import { Spirit } from '../../core/services/single-player.service';
import { CardDescriptionHelper, DescriptionPart } from '../../core/utils/card-description.helper';

@Component({
  selector: 'app-card-display',
  templateUrl: './card-display.html',
  styleUrl: './card-display.scss',
})
export class CardDisplayComponent {
  readonly cardId = input<number>(0);
  readonly borderColor = input<string>('black');
  readonly characterId = input<number>(0);
  readonly name = input<string>('');
  readonly description = input<string>('');
  readonly manaCost = input<number>(0);
  readonly mana = input<number>(0);
  readonly attack = input<number>(0);
  readonly armor = input<number>(0);
  readonly magicAttack = input<number>(0);
  readonly magicArmor = input<number>(0);
  readonly defense = input<number>(0);
  readonly healing = input<number>(0);
  readonly poison = input<number>(0);
  readonly rarity = input<number>(-1);
  readonly permanent = input<boolean>(false);
  readonly draw = input<number>(0);
  readonly strength = input<number>(0);
  readonly effectMultiplier = input<number>(0);
  readonly momentum = input<number>(0);
  readonly daggers = input<number>(0);
  readonly spirit = input<Spirit>(0);
  readonly fire = input<boolean>(false);
  readonly water = input<boolean>(false);
  readonly earth = input<boolean>(false);
  readonly air = input<boolean>(false);

  protected readonly Assets = Assets;
  protected readonly Spirits = Spirit;

  protected readonly picture = computed(() => {
    const role = CardDescriptionHelper.resolveRole(this.characterId());
    if (!role || !this.cardId()) {
      return Assets.Images.Cards.Default;
    }
    return 'assets/images/cards/' + role + '/' + this.cardId() + '.webp';
  });

  protected readonly rarityImage = computed(() => {
    const r = this.rarity();
    if (r < 0) return '';
    return (
      Assets.Images.Cards.Display.Rarity[
        ('Rarity' + r) as keyof typeof Assets.Images.Cards.Display.Rarity
      ] ?? ''
    );
  });

  protected readonly descriptionLineCount = computed(() => {
    const desc = this.description();
    const occurrences = (desc.match(/\{\{NL\}\}/g) || []).length;
    return occurrences + 1;
  });

  protected readonly parsedDescription = computed<DescriptionPart[]>(() => {
    return CardDescriptionHelper.parseDescription(this.description(), {
      attack: this.attack(),
      armor: this.armor(),
      magicAttack: this.magicAttack(),
      magicArmor: this.magicArmor(),
      healing: this.healing(),
      poison: this.poison(),
      mana: this.mana(),
      draw: this.draw(),
      strength: this.strength(),
      defense: this.defense(),
      effectMultiplier: this.effectMultiplier(),
    });
  });

  spiritIcon(): string {
    return CardDescriptionHelper.getSpiritIcon(this.spirit());
  }
}
