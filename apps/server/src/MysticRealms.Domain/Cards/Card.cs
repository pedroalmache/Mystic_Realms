namespace MysticRealms.Domain.Cards;

public class Card
{
    public int Id { get; private set; }
    public int CharacterId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int ManaCost { get; private set; }
    public int Rarity { get; private set; }
    public int Attack { get; private set; }
    public int MagicAttack { get; private set; }
    public int Armor { get; private set; }
    public int MagicArmor { get; private set; }
    public int Mana { get; private set; }
    public int Strength { get; private set; }
    public int Defense { get; private set; }
    public int Draw { get; private set; }
    public int EffectMultiplier { get; private set; }
    public int AscendedManaCost { get; private set; }
    public int AscendedAttack { get; private set; }
    public int AscendedMagicAttack { get; private set; }
    public int AscendedArmor { get; private set; }
    public int AscendedMagicArmor { get; private set; }
    public int AscendedMana { get; private set; }
    public int AscendedStrength { get; private set; }
    public int AscendedDefense { get; private set; }
    public int AscendedDraw { get; private set; }
    public int AscendedEffectMultiplier { get; private set; }
    public int Heal { get; private set; }
    public int AscendedHeal { get; private set; }
    public int Poison { get; private set; }
    public int AscendedPoison { get; private set; }
    public int LifeCost { get; private set; }
    public int AscendedLifeCost { get; private set; }

    public int PlayerTargets { get; private set; } = 0;
    public int AscendedPlayerTargets { get; private set; } = 0;
    public int EnemyTargets  { get; private set; } = 0;
    public int AscendedEnemyTargets  { get; private set; } = 0;
    public bool? Permanent { get; private set; }
    
    //Warrior
    public int? Momentum { get; private set; } = 0;
    
    //Assassin
    public int? Daggers { get; private set; } = 0;

    //Druid
    public Enums.Spirit? Spirit { get; private set; } = Enums.Spirit.None;
    
    //Elementalist
    public bool? Fire { get; private set; }
    public bool? Water { get; private set; }
    public bool? Air { get; private set; }
    public bool? Earth { get; private set; }

    private Card() { }
}
