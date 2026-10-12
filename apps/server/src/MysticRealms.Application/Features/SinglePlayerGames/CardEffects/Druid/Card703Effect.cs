using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card703Effect : ICardEffect
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

        foreach (var pm in partyMemberTargets)
        {
            pm.ApplyStrength(card.Strength);
        }

        if (partyMemberTargets.Count > 0)
        {
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Strength,
                savedGame,
                partyMembers: partyMemberTargets,
                enemies: new List<EnemyDto>()));
        }
    }
}
