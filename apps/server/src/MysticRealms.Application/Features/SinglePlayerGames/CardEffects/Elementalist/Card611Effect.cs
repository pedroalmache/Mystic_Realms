using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card611Effect : ICardEffect
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

        caster.ApplyArmor(card.Armor);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Armor,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster }));

        caster.Earth = true;
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Element,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster }));
    }
}
