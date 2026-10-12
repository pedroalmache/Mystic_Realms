using MysticRealms.Domain.Dungeons;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.Dtos;

// ── Snapshot records used by GameAction ────────────────────────────────────
// These are intentionally separate from the mutable class DTOs in SavedGameDto
// so that every GameAction captures the state as it was at creation time.

public record InitiativeRecord(
    InitiativeType Type,
    Guid? EnemyInstanceId,
    int? PlayerInstanceId,
    string Name,
    int Speed);

public record PartyMemberRecord(
    int PartyMemberId,
    int PlayerId,
    Guid Identifier,
    string Color,
    int ClassId,
    Gender Gender,
    int Health,
    int MaxHealth,
    int Mana,
    int Speed,
    string ClassName,
    int Armor,
    int MagicArmor,
    int Strength,
    int Defense,
    int? Momentum,
    int? Daggers = null,
    Spirit? Spirit = null,
    bool Fire = false,
    bool Water = false,
    bool Earth = false,
    bool Air = false);

public record EnemyRecord(
    Guid InstanceId,
    int Id,
    string Name,
    int HitPoints,
    int MaxHitPoints,
    int Speed,
    int Armor,
    int MagicArmor,
    int Poison);

public record CardRecord(
    int Id,
    int PlayerId,
    int CardId,
    Guid InstanceId,
    int PartyMemberId,
    string Color,
    string Name,
    string Description,
    int Rarity,
    bool IsAscended,
    int UseCount,
    int ManaCost,
    int Attack,
    int MagicAttack,
    int Armor,
    int MagicArmor,
    int Mana,
    int Strength,
    int Defense,
    int Draw,
    int EffectMultiplier,
    int Heal,
    int Poison,
    int LifeCost,
    int PlayerTargets,
    int EnemyTargets,
    bool Permanent = false,
    int? ClassId = null,
    int? Momentum = null,
    int? Daggers = null,
    Spirit? Spirit = null,
    bool? Fire = false,
    bool? Water = false,
    bool? Earth = false,
    bool? Air = false);

public record RoomRecord(
    RoomType Type,
    RewardType Reward,
    string Key,
    int BackgroundId);

public record BoardCardSnapshot(
    CardRecord Card,
    int PartyMemberId,
    EntryPointType EntryPoint);

// ── GameAction and its player snapshot ─────────────────────────────────────

public record GameAction(
    GameActionType Type,
    int? Turn = null,
    IList<PlayerGameAction>? Players = null,
    IList<InitiativeRecord>? Initiative = null,
    IList<CardRecord>? Card = null,
    IList<PartyMemberRecord>? PartyMembers = null,
    IList<EnemyRecord>? Enemies = null,
    RoomRecord? Room = null,
    IList<BoardCardSnapshot>? Board = null);

public record PlayerGameAction(int Id, string UserId, List<CardRecord> Hand, int Mana);
