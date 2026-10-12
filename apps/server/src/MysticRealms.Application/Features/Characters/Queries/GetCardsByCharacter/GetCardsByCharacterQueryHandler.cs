using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;

namespace MysticRealms.Application.Features.Characters.Queries.GetCardsByCharacter;

public class GetCardsByCharacterQueryHandler(ICardRepository cardRepository)
    : IRequestHandler<GetCardsByCharacterQuery, List<CardRecord>>
{
    public async Task<List<CardRecord>> Handle(GetCardsByCharacterQuery request, CancellationToken cancellationToken)
    {
        var cards = await cardRepository.GetByClassIdAsync(request.CharacterId, cancellationToken);

        var result = new List<CardRecord>();

        foreach (var c in cards
                     .OrderBy(c => c.Id))
                     //.OrderBy(c => c.Rarity).ThenBy(c => c.Name))
        {
            result.Add(new CardRecord(
                Id: 0,
                PlayerId: 0,
                CardId: c.Id,
                InstanceId: Guid.Empty,
                PartyMemberId: 0,
                Color: string.Empty,
                Name: c.Name,
                Description: c.Description,
                Rarity: c.Rarity,
                IsAscended: false,
                UseCount: 0,
                ManaCost: c.ManaCost,
                Attack: c.Attack,
                MagicAttack: c.MagicAttack,
                Armor: c.Armor,
                MagicArmor: c.MagicArmor,
                Mana: c.Mana,
                Strength: c.Strength,
                Defense: c.Defense,
                Draw: c.Draw,
                EffectMultiplier: c.EffectMultiplier,
                Heal: c.Heal,
                Poison: c.Poison,
                LifeCost: c.LifeCost,
                PlayerTargets: c.PlayerTargets,
                EnemyTargets: c.EnemyTargets,
                Permanent: c.Permanent ??  false,
                ClassId: c.CharacterId,
                Momentum: c.Momentum,
                Daggers: c.Daggers,
                Spirit: c.Spirit,
                Fire: c.Fire,
                Water: c.Water,
                Earth: c.Earth,
                Air: c.Air
            ));
        }

        return result;
    }
}
