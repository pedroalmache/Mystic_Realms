using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card715Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        if (caster is null)
            return;

        var aliveMembers = savedGame.PartyMembers.Where(pm => pm.Health > 0).ToList();

        foreach (var pm in aliveMembers)
        {
            pm.ApplyStrength(card.Strength);
        }

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Strength,
            savedGame,
            partyMembers: aliveMembers,
            enemies: new List<EnemyDto>()));
    }
}
