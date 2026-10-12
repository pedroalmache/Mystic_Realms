using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card616Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        if (enemyTargets.Count != 1)
            throw new InvalidOperationException("Steam Burst requires exactly one enemy target.");

        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);

        // Record Attack action
        var casterList = caster is not null
            ? new List<PartyMemberDto> { caster }
            : new List<PartyMemberDto>();

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Attack,
            savedGame,
            partyMembers: casterList));

        // Base Magic Damage (Strength is used as an additive bonus for all direct card damage in this project)
        var damage = card.MagicAttack + (caster?.Strength ?? 0);
        enemyTargets[0].ApplyMagicDamage(damage);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.MagicDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: enemyTargets));

        // Synergy: Fire + Water
        if (caster is not null && caster.Fire && caster.Water)
        {
            // Extra Magic Damage
            enemyTargets[0].ApplyMagicDamage(damage);
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.MagicDamage,
                savedGame,
                partyMembers: new List<PartyMemberDto>(),
                enemies: enemyTargets));

            // Consume elements
            caster.Fire = false;
            caster.Water = false;
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Element,
                savedGame,
                partyMembers: casterList));
        }
    }
}
