using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card609Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);

        foreach (var pm in partyMemberTargets)
        {
            pm.Heal(card.Heal);
        }

        if (partyMemberTargets.Count > 0)
        {
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Healing,
                savedGame,
                partyMembers: partyMemberTargets));
        }

        if (caster is not null)
        {
            caster.Water = true;
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Element,
                savedGame,
                partyMembers: new List<PartyMemberDto> { caster }));
        }
    }
}
