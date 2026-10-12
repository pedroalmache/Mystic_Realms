using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card723Effect : ICardEffect
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

        caster.ApplyStrength(card.Strength);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Strength,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster },
            enemies: new List<EnemyDto>()));
    }
}
