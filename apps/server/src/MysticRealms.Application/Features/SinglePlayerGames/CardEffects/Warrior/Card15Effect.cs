using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Warrior;

public class Card15Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var aliveMembers = savedGame.PartyMembers.Where(pm => pm.Health > 0).ToList();

        foreach (var partyMember in aliveMembers)
            partyMember.ApplyDefense(card.Defense);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Defense,
            savedGame,
            partyMembers: aliveMembers,
            enemies: new List<EnemyDto>()));
    }
}
