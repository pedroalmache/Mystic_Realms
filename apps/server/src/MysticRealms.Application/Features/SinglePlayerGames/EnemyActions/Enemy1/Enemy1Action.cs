using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.EnemyActions.Enemy1;

public class Enemy1Action : IEnemyAction
{
    private const int MinAmount = 5;
    private const int MaxAmount = 11;

    public EnemyPlannedAction Plan(EnemyDto enemy, SavedGameDto savedGame, Random rng)
    {
        var roll = rng.Next(0, 100);

        if (roll < 35)
            return PlanAttack(savedGame, rng);

        if (roll < 70)
            return PlanArmor(rng);

        return PlanHealing(savedGame, rng);
    }

    public void Execute(EnemyDto enemy, SavedGameDto savedGame, EnemyPlannedAction plan)
    {
        switch (plan.Kind)
        {
            case EnemyPlannedActionKind.PhysicalAttack:
                ExecuteAttack(enemy, savedGame, plan);
                break;
            case EnemyPlannedActionKind.PhysicalArmor:
                ExecuteArmor(enemy, savedGame, plan);
                break;
            case EnemyPlannedActionKind.Heal:
                ExecuteHealing(savedGame, plan);
                break;
        }
    }

    private static EnemyPlannedAction PlanAttack(SavedGameDto savedGame, Random rng)
    {
        var aliveMembers = savedGame.PartyMembers.Where(pm => pm.Health > 0).ToList();
        int? targetPartyMemberId = null;
        if (aliveMembers.Count > 0)
            targetPartyMemberId = aliveMembers[rng.Next(aliveMembers.Count)].PartyMemberId;

        return new EnemyPlannedAction
        {
            Kind = EnemyPlannedActionKind.PhysicalAttack,
            Amount = rng.Next(MinAmount, MaxAmount),
            TargetPartyMemberId = targetPartyMemberId
        };
    }

    private static EnemyPlannedAction PlanArmor(Random rng) =>
        new()
        {
            Kind = EnemyPlannedActionKind.PhysicalArmor,
            Amount = rng.Next(MinAmount, MaxAmount)
        };

    private static EnemyPlannedAction PlanHealing(SavedGameDto savedGame, Random rng)
    {
        var target = savedGame.Enemies
            .Where(e => e.HitPoints > 0 && e.HitPoints < e.MaxHitPoints)
            .OrderBy(e => e.HitPoints)
            .FirstOrDefault();

        if (target is null)
            return PlanArmor(rng);

        return new EnemyPlannedAction
        {
            Kind = EnemyPlannedActionKind.Heal,
            Amount = rng.Next(MinAmount, MaxAmount),
            TargetEnemyInstanceId = target.InstanceId
        };
    }

    private static void ExecuteAttack(EnemyDto enemy, SavedGameDto savedGame, EnemyPlannedAction plan)
    {
        var target = plan.TargetPartyMemberId is int partyMemberId
            ? savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == partyMemberId && pm.Health > 0)
            : null;

        target ??= savedGame.PartyMembers.FirstOrDefault(pm => pm.Health > 0);
        if (target is null) return;

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.EnemyAttack,
            savedGame,
            enemies: new List<EnemyDto> { enemy }));

        target.ApplyPhysicalDamage(plan.Amount);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.PhysicalDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto> { target },
            enemies: new List<EnemyDto>()));
    }

    private static void ExecuteArmor(EnemyDto enemy, SavedGameDto savedGame, EnemyPlannedAction plan)
    {
        enemy.ApplyArmor(plan.Amount);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.EnemyArmor,
            savedGame,
            enemies: new List<EnemyDto> { enemy }));
    }

    private static void ExecuteHealing(SavedGameDto savedGame, EnemyPlannedAction plan)
    {
        var target = plan.TargetEnemyInstanceId is Guid instanceId
            ? savedGame.Enemies.FirstOrDefault(e => e.InstanceId == instanceId && e.HitPoints > 0)
            : null;

        if (target is null || target.HitPoints >= target.MaxHitPoints)
        {
            // Planned heal target is gone or full — fall back to armor on the acting enemy if present in log context.
            // Caller executes heal for the planned target; if invalid, no-op (armor fallback already applied at plan time).
            target = savedGame.Enemies
                .Where(e => e.HitPoints > 0 && e.HitPoints < e.MaxHitPoints)
                .OrderBy(e => e.HitPoints)
                .FirstOrDefault();

            if (target is null) return;
        }

        target.Heal(plan.Amount);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.EnemyHealing,
            savedGame,
            enemies: new List<EnemyDto> { target }));
    }
}
