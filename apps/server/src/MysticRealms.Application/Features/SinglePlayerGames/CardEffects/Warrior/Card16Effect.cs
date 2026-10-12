using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Warrior;

public class Card16Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        if (partyMemberTargets.Count != 2)
            throw new InvalidOperationException("Guardian Angel requires exactly two party member targets.");

        var targets = partyMemberTargets;

        foreach (var target in targets)
            target.ApplyArmor(card.Armor);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Armor,
            savedGame,
            partyMembers: targets));
    }
}
