using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.EnemyActions.Enemy1;

public class Enemy1Action : IEnemyAction
{
    private const int MinAmount = 5;
    private const int MaxAmount = 11;

    public void Execute(EnemyDto enemy, SavedGameDto savedGame)
    {
        var roll = Random.Shared.Next(0, 100);

        if (roll < 35)
            ExecuteAttack(enemy, savedGame);
        else if (roll < 70)
            ExecuteArmor(enemy, savedGame);
        else
            ExecuteHealing(enemy, savedGame);
    }

    private static void ExecuteAttack(EnemyDto enemy, SavedGameDto savedGame)
    {
        var aliveMembers = savedGame.PartyMembers.Where(pm => pm.Health > 0).ToList();
        if (aliveMembers.Count == 0) return;

        var target = aliveMembers[Random.Shared.Next(aliveMembers.Count)];
        var damage = Random.Shared.Next(MinAmount, MaxAmount);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.EnemyAttack,
            savedGame,
            enemies: new List<EnemyDto> { enemy }));

        target.ApplyPhysicalDamage(damage);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.PhysicalDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto> { target },
            enemies: new List<EnemyDto>()));
    }

    private static void ExecuteArmor(EnemyDto enemy, SavedGameDto savedGame)
    {
        var amount = Random.Shared.Next(MinAmount, MaxAmount);
        enemy.ApplyArmor(amount);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.EnemyArmor,
            savedGame,
            enemies: new List<EnemyDto> { enemy }));
    }

    private static void ExecuteHealing(EnemyDto enemy, SavedGameDto savedGame)
    {
        var target = savedGame.Enemies
            .Where(e => e.HitPoints > 0 && e.HitPoints < e.MaxHitPoints)
            .OrderBy(e => e.HitPoints)
            .FirstOrDefault();

        if (target is null)
        {
            // All enemies are at max HP — do armor instead
            ExecuteArmor(enemy, savedGame);
            return;
        }

        var amount = Random.Shared.Next(MinAmount, MaxAmount);
        target.Heal(amount);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.EnemyHealing,
            savedGame,
            enemies: new List<EnemyDto> { target }));
    }
}
