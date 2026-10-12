using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Warrior;

public class Card3Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var aliveMembers = savedGame.PartyMembers.Where(pm => pm.Health > 0).ToList();

        foreach (var partyMember in aliveMembers)
            partyMember.ApplyArmor(card.Armor);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Armor,
            savedGame,
            partyMembers: aliveMembers));
    }
}
