using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands;

public class PredatorInstinctBoardEffect : IBoardEffect
{
    public void React(BoardCardRecord boardCard, SavedGameDto savedGame, List<GameAction> playActions)
    {
        bool hasAttack = playActions.Any(a => a.Type == GameActionType.Attack);
        if (hasAttack)
        {
            var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == boardCard.PartyMemberId);
            if (caster?.Spirit == Spirit.Wolf)
            {
                var playerIndex = savedGame.Players.FindIndex(p => p.Id == caster.PlayerId);
                if (playerIndex != -1)
                {
                    CombatTurnHelper.DrawCardsForPlayer(savedGame, playerIndex, 1);
                }
            }
        }
    }
}
