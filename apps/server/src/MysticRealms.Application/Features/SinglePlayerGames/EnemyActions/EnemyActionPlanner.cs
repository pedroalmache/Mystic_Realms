using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Dungeons;

namespace MysticRealms.Application.Features.SinglePlayerGames.EnemyActions;

public static class EnemyActionPlanner
{
    public static void PlanForTurn(SavedGameDto savedGame)
    {
        var roomKey = savedGame.CurrentRoom?.Key ?? string.Empty;
        var turn = savedGame.Turn;

        foreach (var enemy in savedGame.Enemies)
        {
            if (enemy.HitPoints <= 0)
            {
                enemy.PlannedAction = null;
                continue;
            }

            var seed = DungeonGenerator.GetStableHash($"{roomKey}|{turn}|{enemy.InstanceId}");
            var rng = new Random(seed);
            var action = EnemyActionFactory.GetAction(enemy.Id);
            enemy.PlannedAction = action.Plan(enemy, savedGame, rng);
        }
    }

    public static EnemyPlannedAction EnsurePlan(EnemyDto enemy, SavedGameDto savedGame)
    {
        if (enemy.PlannedAction is not null)
            return enemy.PlannedAction;

        var roomKey = savedGame.CurrentRoom?.Key ?? string.Empty;
        var seed = DungeonGenerator.GetStableHash($"{roomKey}|{savedGame.Turn}|{enemy.InstanceId}");
        var rng = new Random(seed);
        var action = EnemyActionFactory.GetAction(enemy.Id);
        enemy.PlannedAction = action.Plan(enemy, savedGame, rng);
        return enemy.PlannedAction;
    }
}
