using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects;

public class BasicMagicDefenseEffect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        if (caster is not null)
        {
            caster.ApplyMagicArmor(card.MagicArmor);

            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.MagicArmor,
                savedGame,
                partyMembers: new List<PartyMemberDto> { caster }));
        }
    }
}
