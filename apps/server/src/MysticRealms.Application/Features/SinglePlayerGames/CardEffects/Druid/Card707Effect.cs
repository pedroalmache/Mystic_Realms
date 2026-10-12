using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card707Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);

        var aliveEnemies = savedGame.Enemies.Where(e => e.HitPoints > 0).ToList();

        foreach (var target in aliveEnemies)
        {
            target.ApplyPoison(card.Poison, caster?.Identifier ?? Guid.Empty);
        }

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Poison,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: aliveEnemies));
    }
}
