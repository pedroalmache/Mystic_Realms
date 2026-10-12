using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetPlayerDeck;

public class GetPlayerDeckQueryHandler(
    IGameRepository gameRepository,
    IPartyMemberRepository partyMemberRepository)
    : IRequestHandler<GetPlayerDeckQuery, GetPlayerDeckResult?>
{
    public async Task<GetPlayerDeckResult?> Handle(
        GetPlayerDeckQuery request,
        CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(request.GameId, cancellationToken);
        if (game is null)
            return null;

        var player = game.Players.FirstOrDefault(p => p.Id == request.PlayerId && p.UserId == request.UserId);
        if (player is null)
            return null;

        var classIdByPartyMemberId = (await partyMemberRepository.GetByGameIdAsync(request.GameId, cancellationToken))
            .Where(pm => pm.PlayerId == player.Id)
            .ToDictionary(pm => pm.Id, pm => pm.ClassId);

        var cards = player.Deck
            .OrderBy(d => d.Card.Name)
            .ThenBy(d => d.Id)
            .Select(d =>
            {
                classIdByPartyMemberId.TryGetValue(d.PartyMemberId, out var classId);
                return new CardDto(
                    d.Id,
                    player.Id,
                    d.CardId,
                    d.InstanceId,
                    d.PartyMemberId,
                    classId,
                    d.Color,
                    d.Card.Name,
                    d.Card.Description,
                    d.Card.Rarity,
                    d.IsAscended,
                    d.UseCount,
                    d.ManaCost,
                    d.Attack,
                    d.MagicAttack,
                    d.Armor,
                    d.MagicArmor,
                    d.Mana,
                    d.Strength,
                    d.Defense,
                    d.Draw,
                    d.EffectMultiplier,
                    d.Heal,
                    d.Poison,
                    d.LifeCost,
                    d.PlayerTargets,
                    d.EnemyTargets,
                    d.Card.Permanent ?? false,

                    d.Card.Momentum, //warrior
                    d.Card.Daggers, //Assassin
                    d.Card.Spirit, //Druid
                    d.Card.Fire, //Elementalist
                    d.Card.Water,
                    d.Card.Earth,
                    d.Card.Air
                                    );
            })
            .ToList();

        return new GetPlayerDeckResult(cards);
    }
}
