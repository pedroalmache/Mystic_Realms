using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Warrior;

public class Card10Effect : ICardEffect
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

        if (card.Draw > 0)
        {
            var playerIndex = savedGame.Players.FindIndex(p => p.Id == card.PlayerId);
            if (playerIndex >= 0)
                CombatTurnHelper.DrawCardsForPlayer(savedGame, playerIndex, card.Draw);
        }
    }
}
