using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card619Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);

        var casterList = caster is not null
            ? new List<PartyMemberDto> { caster }
            : new List<PartyMemberDto>();

        // Initial Attack Action
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Attack,
            savedGame,
            partyMembers: casterList));

        // Damage calculation
        var damage = card.MagicAttack + (caster?.Strength ?? 0);

        // First wave: all targeted enemies
        if (enemyTargets.Count > 0)
        {
            foreach (var enemy in enemyTargets)
            {
                enemy.ApplyMagicDamage(damage);
            }

            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.MagicDamage,
                savedGame,
                partyMembers: new List<PartyMemberDto>(),
                enemies: enemyTargets));
        }

        // Synergy: Fire + Water
        if (caster is not null && caster.Fire && caster.Water)
        {
            // Repeat attack for enemies in target list still alive
            var remainingEnemies = enemyTargets.Where(e => e.HitPoints > 0).ToList();
            if (remainingEnemies.Count > 0)
            {
                foreach (var enemy in remainingEnemies)
                {
                    enemy.ApplyMagicDamage(damage);
                }

                savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                    GameActionType.MagicDamage,
                    savedGame,
                    partyMembers: new List<PartyMemberDto>(),
                    enemies: remainingEnemies));
            }

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
