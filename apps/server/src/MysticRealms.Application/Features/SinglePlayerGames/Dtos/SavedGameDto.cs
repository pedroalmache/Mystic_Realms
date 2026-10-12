using MysticRealms.Domain.Games;
using System.Text.Json;
using System.Text.Json.Serialization;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.Dtos;

public class SavedGameDto
{
    public RoomDto? CurrentRoom { get; set; }
    public List<PlayerDto> Players { get; set; } = new();
    public List<EnemyDto>  Enemies { get; set; } = new();
    public List<PartyMemberDto> PartyMembers { get; set; } = new();
    public List<InitiativeDto> Initiatives { get; set; } = new();
    public List<GameAction> ActionLog { get; set; } = new();
    public int Turn { get; set; } = 0;
    public List<BoardCardRecord> Board { get; set; } = [];

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public string ToJson() => JsonSerializer.Serialize(this, SerializerOptions);

    public static SavedGameDto? FromJson(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<SavedGameDto>(json, SerializerOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }
}

public class RoomDto
{
    public RoomDto() { }

    public RoomDto(RoomType type, RewardType reward, string key, int backgroundId)
    {
        Type = type;
        Reward = reward;
        Key = key;
        BackgroundId = backgroundId;
    }

    public RoomType Type { get; set; }
    public RewardType Reward { get; set; }
    public string Key { get; set; } = string.Empty;
    public int BackgroundId { get; set; }
}

public class EffectEntry
{
    public EffectEntry() { }

    public EffectEntry(int amount, Guid source)
    {
        Amount = amount;
        Source = source;
    }

    public int Amount { get; set; }
    public Guid Source { get; set; }
}

public class PlayerDto
{
    public PlayerDto() { }

    public PlayerDto(int id, string userId, List<CardDto> deck, List<CardDto> drawPile, List<CardDto> hand, List<CardDto> discardPile, int mana, int speed, string gamerTag)
    {
        Id = id;
        UserId = userId;
        Deck = deck;
        DrawPile = drawPile;
        Hand = hand;
        DiscardPile = discardPile;
        Mana = mana;
        Speed = speed;
        GamerTag = gamerTag;
    }

    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<CardDto> Deck { get; set; } = new();
    public List<CardDto> DrawPile { get; set; } = new();
    public List<CardDto> Hand { get; set; } = new();
    public List<CardDto> DiscardPile { get; set; } = new();
    public int Mana { get; set; }
    public int Speed { get; set; }
    public string GamerTag { get; set; } = string.Empty;

    public void MoveCardToDiscardPile(Guid cardInstanceId)
    {
        var card = Hand.FirstOrDefault(c => c.InstanceId == cardInstanceId);
        if (card is null) return;
        Hand.Remove(card);
        DiscardPile.Add(card);
    }
}

public class EnemyDto
{
    public EnemyDto() { }

    public EnemyDto(Guid instanceId, int id, string name, int hitPoints, int maxHitPoints, int speed)
    {
        InstanceId = instanceId;
        Id = id;
        Name = name;
        HitPoints = hitPoints;
        MaxHitPoints = maxHitPoints;
        Speed = speed;
    }

    public Guid InstanceId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int HitPoints { get; set; }
    public int MaxHitPoints { get; set; }
    public int Speed { get; set; }
    public List<EffectEntry> Armor { get; set; } = new();
    public int TotalArmor => Armor.Sum(e => e.Amount);
    public List<EffectEntry> Poison { get; set; } = new();
    public int TotalPoison => Poison.Sum(e => e.Amount);
    public List<EffectEntry> MagicArmor { get; set; } = new();
    public int TotalMagicArmor => MagicArmor.Sum(e => e.Amount);

    /// <summary>
    /// Applies physical damage to this enemy, consuming armor entries from oldest to newest
    /// before reducing hit points.
    /// </summary>
    public void ApplyPhysicalDamage(int damage)
    {
        var remaining = damage;
        foreach (var entry in Armor)
        {
            if (remaining <= 0) break;
            var absorbed = Math.Min(remaining, entry.Amount);
            entry.Amount -= absorbed;
            remaining -= absorbed;
        }
        Armor.RemoveAll(e => e.Amount <= 0);
        HitPoints = Math.Max(0, HitPoints - remaining);
    }

    /// <summary>
    /// Applies magic damage to this enemy, consuming magic armor entries from oldest to newest
    /// before reducing hit points.
    /// </summary>
    public void ApplyMagicDamage(int damage)
    {
        var remaining = damage;
        foreach (var entry in MagicArmor)
        {
            if (remaining <= 0) break;
            var absorbed = Math.Min(remaining, entry.Amount);
            entry.Amount -= absorbed;
            remaining -= absorbed;
        }
        MagicArmor.RemoveAll(e => e.Amount <= 0);
        HitPoints = Math.Max(0, HitPoints - remaining);
    }

    /// <summary>
    /// Adds an armor <see cref="EffectEntry"/> sourced by this enemy.
    /// </summary>
    public void ApplyArmor(int amount)
    {
        Armor.Add(new EffectEntry(amount, InstanceId));
    }

    /// <summary>
    /// Adds a magic armor <see cref="EffectEntry"/> sourced by this enemy.
    /// </summary>
    public void ApplyMagicArmor(int amount)
    {
        MagicArmor.Add(new EffectEntry(amount, InstanceId));
    }

    /// <summary>
    /// Adds a poison <see cref="EffectEntry"/> sourced by the caster.
    /// </summary>
    public void ApplyPoison(int amount, Guid sourceId)
    {
        Poison.Add(new EffectEntry(amount, sourceId));
    }

    /// <summary>
    /// Heals this enemy by <paramref name="amount"/>, capped at <see cref="MaxHitPoints"/>.
    /// </summary>
    public void Heal(int amount)
    {
        HitPoints = Math.Min(MaxHitPoints, HitPoints + amount);
    }
}

public class PartyMemberDto
{
    public PartyMemberDto() { }

    public PartyMemberDto(int partyMemberId, int playerId, Guid identifier, string color, int classId, Gender gender, int health, int maxHealth, int mana, int speed, string className = "", int defense = 0, int strength = 0, Spirit? spirit = null, bool fire = false, bool water = false, bool earth = false, bool air = false)
    {
        PartyMemberId = partyMemberId;
        PlayerId = playerId;
        Identifier = identifier;
        Color = color;
        ClassId = classId;
        Gender = gender;
        Health = health;
        MaxHealth = maxHealth;
        Mana = mana;
        Speed = speed;
        ClassName = className;
        Defense = defense;
        Strength = strength;
        Spirit = spirit;
        Fire = fire;
        Water = water;
        Earth = earth;
        Air = air;
    }

    public int PartyMemberId { get; set; }
    public int PlayerId { get; set; }
    public Guid Identifier { get; set; }
    public string Color { get; set; } = string.Empty;
    public int ClassId { get; set; }
    public Gender Gender { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Mana { get; set; }
    public int Speed { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int Defense { get; set; }
    public int Strength { get; set; }
    public List<EffectEntry> Armor { get; set; } = new();
    public int TotalArmor => Armor.Sum(e => e.Amount);
    public List<EffectEntry> Poison { get; set; } = new();
    public int TotalPoison => Poison.Sum(e => e.Amount);
    public List<EffectEntry> MagicArmor { get; set; } = new();
    public int TotalMagicArmor => MagicArmor.Sum(e => e.Amount);
    public int? Momentum { get; set; }
    public int? Daggers { get; set; }
    public Spirit? Spirit { get; set; }
    public bool Fire { get; set; }
    public bool Water { get; set; }
    public bool Earth { get; set; }
    public bool Air { get; set; }

    /// <summary>
    /// Adds an armor <see cref="EffectEntry"/> sourced by this party member.
    /// The effective amount is <paramref name="baseAmount"/> plus this member's <see cref="Defense"/>.
    /// </summary>
    public void ApplyArmor(int baseAmount)
    {
        Armor.Add(new EffectEntry(baseAmount + Defense, Identifier));
    }

    /// <summary>
    /// Adds a magic armor <see cref="EffectEntry"/> sourced by this party member.
    /// The effective amount is <paramref name="baseAmount"/> plus this member's <see cref="Defense"/>.
    /// </summary>
    public void ApplyMagicArmor(int baseAmount)
    {
        MagicArmor.Add(new EffectEntry(baseAmount + Defense, Identifier));
    }

    /// <summary>
    /// Increases this party member's strength by <paramref name="amount"/>.
    /// </summary>
    public void ApplyStrength(int amount)
    {
        Strength = Math.Max(0, Strength + amount);
    }

    /// <summary>
    /// Increases this party member's defense by <paramref name="amount"/>.
    /// </summary>
    public void ApplyDefense(int amount)
    {
        Defense = Math.Max(0, Defense + amount);
    }

    /// <summary>
    /// Applies physical damage to this party member, consuming armor entries from oldest to newest
    /// before reducing health.
    /// </summary>
    public void ApplyPhysicalDamage(int damage)
    {
        var remaining = damage;
        foreach (var entry in Armor)
        {
            if (remaining <= 0) break;
            var absorbed = Math.Min(remaining, entry.Amount);
            entry.Amount -= absorbed;
            remaining -= absorbed;
        }
        Armor.RemoveAll(e => e.Amount <= 0);
        Health = Math.Max(0, Health - remaining);
    }

    /// <summary>
    /// Applies magic damage to this party member, consuming magic armor entries from oldest to newest
    /// before reducing health.
    /// </summary>
    public void ApplyMagicDamage(int damage)
    {
        var remaining = damage;
        foreach (var entry in MagicArmor)
        {
            if (remaining <= 0) break;
            var absorbed = Math.Min(remaining, entry.Amount);
            entry.Amount -= absorbed;
            remaining -= absorbed;
        }
        MagicArmor.RemoveAll(e => e.Amount <= 0);
        Health = Math.Max(0, Health - remaining);
    }

    public void ApplyMomentum(int momentum)
    {
        Momentum = (Momentum ?? 0) +  momentum;
    }
    public void ApplySpirit(Spirit spirit)
    {
        Spirit = spirit;
    }

    /// <summary>
    /// Heals this party member by <paramref name="amount"/>, capped at <see cref="MaxHealth"/>.
    /// </summary>
    public void Heal(int amount)
    {
        Health = Math.Min(MaxHealth, Health + amount);
    }
}

public class InitiativeDto
{
    public InitiativeDto() { }

    public InitiativeDto(InitiativeType type, Guid? enemyInstanceId, int? playerInstanceId, string name, int speed)
    {
        Type = type;
        EnemyInstanceId = enemyInstanceId;
        PlayerInstanceId = playerInstanceId;
        Name = name;
        Speed = speed;
    }

    public InitiativeType Type { get; set; }
    public Guid? EnemyInstanceId { get; set; }
    public int? PlayerInstanceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Speed { get; set; }
}

public class BoardCardRecord
{
    public CardDto Card { get; set; } = null!;
    public int PartyMemberId { get; set; }
    public EntryPointType EntryPoint { get; set; }
}

public class CardDto
{
    public CardDto() { }

    public CardDto(
        int id, int playerId, int cardId, Guid instanceId, int partyMemberId, int classId,
        string color, string name, string description, int rarity, bool isAscended,
        int useCount,
        int manaCost,
        int attack, int magicAttack, int armor, int magicArmor, int mana, int strength, int defense, int draw, int effectMultiplier, int heal, int poison, int lifeCost,
        int playerTargets, int enemyTargets, bool permanent,
        int? momentum,
        int? daggers,
        Spirit? spirit = null,
        bool? fire = null,
        bool? water = null,
        bool? earth = null,
        bool? air = null)
    {
        Id = id;
        PlayerId = playerId;
        CardId = cardId;
        InstanceId = instanceId;
        PartyMemberId = partyMemberId;
        ClassId = classId;
        Color = color;
        Name = name;
        Description = description;
        Rarity = rarity;
        IsAscended = isAscended;
        UseCount = useCount;
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
        Permanent = permanent;
        Momentum = momentum;
        Daggers =  daggers;
        Spirit = spirit;
        Fire = fire;
        Water = water;
        Earth = earth;
        Air = air;
    }

    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int CardId { get; set; }
    public Guid InstanceId { get; set; }
    public int PartyMemberId { get; set; }
    public int ClassId { get; set; }
    public string Color { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Rarity { get; set; }
    public bool IsAscended { get; set; }
    public int UseCount { get; set; }
    public int ManaCost { get; set; }
    public int Attack { get; set; }
    public int MagicAttack { get; set; }
    public int Armor { get; set; }
    public int MagicArmor { get; set; }
    public int Mana { get; set; }
    public int Strength { get; set; }
    public int Defense { get; set; }
    public int Draw { get; set; }
    public int EffectMultiplier { get; set; }
    public int Heal { get; set; }
    public int Poison { get; set; }
    public int LifeCost { get; set; }
    public int PlayerTargets { get; set; }
    public int EnemyTargets { get; set; }
    public bool Permanent { get; set; }
    
    //warrior
    public int? Momentum { get; set; }
    
    //assassin
    public int? Daggers { get; set; }

    //druid
    public Spirit? Spirit { get; set; }
    
    //elementalist
    public bool? Fire { get; set; }
    public bool? Water { get; set; }
    public bool? Earth { get; set; }
    public bool? Air { get; set; }
}
