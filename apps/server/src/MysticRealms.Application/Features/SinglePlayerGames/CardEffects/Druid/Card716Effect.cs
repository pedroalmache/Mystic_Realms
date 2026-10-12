using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card716Effect : ICardEffect
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

        var strengthToShare = caster.Strength;
        if (strengthToShare <= 0)
            return;

        var otherMembers = savedGame.PartyMembers.Where(pm => pm.PartyMemberId != caster.PartyMemberId && pm.Health > 0).ToList();

        if (otherMembers.Count == 0)
            return;

        foreach (var pm in otherMembers)
        {
            pm.ApplyStrength(strengthToShare);
        }

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Strength,
            savedGame,
            partyMembers: otherMembers,
            enemies: new List<EnemyDto>()));
    }
}
