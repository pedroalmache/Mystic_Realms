using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card718Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);

        // Attack action: snapshot the attacking party member
        var attackPartyMembers = caster is not null
            ? new List<PartyMemberDto> { caster }
            : new List<PartyMemberDto>();
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Attack,
            savedGame,
            partyMembers: attackPartyMembers));

        var damage = card.MagicAttack + (caster?.Strength ?? 0);
        var aliveEnemies = savedGame.Enemies.Where(e => e.HitPoints > 0).ToList();

        // Apply magic damage to each target
        foreach (var target in aliveEnemies)
            target.ApplyMagicDamage(damage);

        // MagicDamage action: snapshot only the targeted enemies
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.MagicDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: aliveEnemies));

        // If Caster Spirit is Deer, Gain 1 Mana
        if (caster is not null && caster.Spirit == Spirit.Deer)
        {
            var owner = savedGame.Players.FirstOrDefault(p => p.Id == card.PlayerId);
            if (owner is not null)
            {
                owner.Mana += card.Mana;
                savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                    GameActionType.Mana,
                    savedGame,
                    partyMembers: new List<PartyMemberDto> { caster }));
                savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateMana, savedGame));
            }
        }
    }
}
