using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card610Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        if (enemyTargets.Count != 1)
            throw new InvalidOperationException("Rock Spike requires exactly one enemy target.");

        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);

        var attackPartyMembers = caster is not null
            ? new List<PartyMemberDto> { caster }
            : new List<PartyMemberDto>();
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Attack,
            savedGame,
            partyMembers: attackPartyMembers));

        var damage = card.MagicAttack + (caster?.Strength ?? 0);
        enemyTargets[0].ApplyMagicDamage(damage);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.MagicDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: enemyTargets));

        if (caster is not null)
        {
            // Synergy check: Earth + Air
            bool hasAir = caster.Air || card.Air == true;

            if (hasAir)
            {
                var playerIndex = savedGame.Players.FindIndex(p => p.Id == caster.PlayerId);
                if (playerIndex != -1)
                {
                    CombatTurnHelper.DrawCardsForPlayer(savedGame, playerIndex, card.Draw);
                }
            }

            caster.Earth = true;
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Element,
                savedGame,
                partyMembers: new List<PartyMemberDto> { caster }));
        }
    }
}
