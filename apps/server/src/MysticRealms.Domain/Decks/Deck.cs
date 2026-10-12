using MysticRealms.Domain.Cards;
using MysticRealms.Domain.Players;

namespace MysticRealms.Domain.Decks;

public class Deck
{
    public int Id { get; private set; }
    public int PlayerId { get; private set; }
    public int CardId { get; private set; }
    public int PartyMemberId { get; private set; }
    public Guid InstanceId { get; private set; }
    public string Color { get; private set; }
    public bool IsAscended { get; private set; }
    public int UseCount { get; private set; }
    public int ManaCost  { get; private set; }
    
    public int Attack  { get; private set; }
    public int MagicAttack  { get; private set; }
    public int Armor  { get; private set; }
    public int MagicArmor  { get; private set; }
    public int Mana  { get; private set; }
    public int Strength  { get; private set; }
    public int Defense { get; private set; }
    public int Draw { get; private set; }
    public int EffectMultiplier { get; private set; }
    public int Heal { get; private set; }
    public int Poison { get; private set; }
    public int LifeCost { get; private set; }
    public int PlayerTargets { get; private set; }
    public int EnemyTargets { get; private set; }

    public Player Player { get; private set; }
    public Card Card { get; private set; }

    private Deck(
        int playerId,
        int cardId,
        int partyMemberId,
        string color,
        int manaCost,
        int attack,
        int magicAttack,
        int armor,
        int magicArmor,
        int mana,
        int strength,
        int defense,
        int draw,
        int effectMultiplier,
        int heal,
        int poison,
        int lifeCost,
        int playerTargets,
        int enemyTargets)
    {
        PlayerId = playerId;
        CardId = cardId;
        PartyMemberId = partyMemberId;
        Color = color;
        IsAscended = false;
        UseCount = 0;
        ManaCost = manaCost;
        Attack = attack;
        MagicAttack = magicAttack;
        Armor = armor;
        MagicArmor = magicArmor;
        Mana = mana;
        Strength = strength;
        Defense = defense;
        Draw = draw;
        EffectMultiplier = effectMultiplier;
        Heal = heal;
        Poison = poison;
        LifeCost = lifeCost;
        PlayerTargets = playerTargets;
        EnemyTargets = enemyTargets;
        InstanceId = Guid.NewGuid();
    }

    public void IncrementUseCount(int amount)
    {
        if (amount > 0)
            UseCount += amount;
    }

    public static Deck Create(
        int playerId,
        int cardId,
        int partyMemberId,
        string color,
        int manaCost,
        int attack,
        int magicAttack,
        int armor,
        int magicArmor,
        int mana,
        int strength,
        int defense,
        int draw,
        int effectMultiplier,
        int heal,
        int poison,
        int lifeCost,
        int playerTargets,
        int enemyTargets
    )
    {
        return new Deck(playerId, cardId, partyMemberId, color, manaCost, attack, magicAttack, armor, magicArmor, mana, strength, defense, draw, effectMultiplier, heal, poison,
            lifeCost, playerTargets, enemyTargets);
    }
}