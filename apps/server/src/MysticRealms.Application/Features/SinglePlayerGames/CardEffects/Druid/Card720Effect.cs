using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card720Effect : ICardEffect
{
    private static readonly Spirit[] Spirits =
    [
        Spirit.Wolf,
        Spirit.Bear,
        Spirit.Deer,
        Spirit.Owl,
        Spirit.Tortoise
    ];

    public void Execute(CardDto card, List<PartyMemberDto> partyMemberTargets, List<EnemyDto> enemyTargets, SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        if (caster is null)
            return;

        var randomSpirit = Spirits[Random.Shared.Next(Spirits.Length)];

        caster.ApplySpirit(randomSpirit);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Spirit,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster },
            enemies: new List<EnemyDto>()));
    }
}
