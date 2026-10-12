using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card606Effect : ICardEffect
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

        caster.ApplyMagicArmor(card.MagicArmor);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.MagicArmor,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster }));

        caster.Fire = true;
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Element,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster }));
    }
}
