using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Commands.NextRoom;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Domain.Games;
using MysticRealms.Domain.PartyMembers;
using MysticRealms.Domain.Players;
using System.Security.Cryptography;
using MysticRealms.Application.Features.SinglePlayerGames.Commands.EndTurn;
using MysticRealms.Domain.Decks;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands.CreateSinglePlayerGame;

public class CreateSinglePlayerGameCommandHandler(
    IMediator mediator,
    IGameRepository gameRepository,
    IPlayerRepository playerRepository,
    IDungeonRepository dungeonRepository,
    IPartyMemberRepository partyMemberRepository,
    ICharacterRepository characterRepository,
    ICardRepository cardRepository,
    IDeckRepository deckRepository)
    : IRequestHandler<CreateSinglePlayerGameCommand, GameDto>
{
    private const string DefaultGamerTag = "Player";

    // Debug override: fill this list with card IDs to force specific cards into every player's deck on game creation.
    // Leave empty for normal deck building.
    private static readonly int[] DebugCardIds = [610, 611,612, 613];

    public async Task<GameDto> Handle(
        CreateSinglePlayerGameCommand request,
        CancellationToken cancellationToken)
    {
        await gameRepository.DeactivateAllSinglePlayerGamesForUserAsync(request.UserId, cancellationToken);

        var game = Game.CreateSinglePlayer();
        game = await gameRepository.AddAsync(game, cancellationToken);

        var seed = request.Seed ?? GenerateCode();
        var dungeon = Dungeon.Create(game.Id, seed);
        await dungeonRepository.AddAsync(dungeon, cancellationToken);

        var player = Player.Create(game.Id, request.UserId, isHost: true);
        player = await playerRepository.AddAsync(player, cancellationToken);

        var partyMembers = await BuildPartyAsync(request.PartyMembers, player.Id, cancellationToken);
        await partyMemberRepository.AddRangeAsync(partyMembers, cancellationToken);
        
        var deckCards = await BuildDeckAsync(partyMembers, cancellationToken);
        await deckRepository.AddRangeAsync(deckCards, cancellationToken);

        await mediator.Send(new NextRoomCommand(game.Id, request.UserId, RoomOptions.Initial, DefaultGamerTag), cancellationToken);
        await mediator.Send(new EndTurnCommand(game.Id, request.UserId));
        return new GameDto(game.Id, seed, partyMembers.Count);
    }

    private static string GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Range(0, 8)
            .Select(_ => chars[RandomNumberGenerator.GetInt32(chars.Length)])
            .ToArray());
    }

    private async Task<List<PartyMember>> BuildPartyAsync(
        List<CreateSinglePlayerPartyMemberDto> partyMemberDtos, 
        int playerId, 
        CancellationToken cancellationToken)
    {
        var characterClasses = await characterRepository.GetAllAsync(cancellationToken);
        return partyMemberDtos.Select(dto =>
        {
            var characterClass = characterClasses.First(f => f.Id == dto.ClassId);
            return PartyMember.Create(
                playerId,
                dto.ClassId,
                dto.Color,
                dto.Gender,
                characterClass.HitPoints,
                characterClass.Speed,
                characterClass.Energy);
        }).ToList();
    }

    private async Task<List<Deck>> BuildDeckAsync(
        List<PartyMember> partyMembers,
        CancellationToken cancellationToken)
    {
        var deck = new List<Deck>();

        int[] copyCounts = partyMembers.Count switch
        {
            4 => [1, 1, 1],
            3 => [2, 1, 1],
            2 => [2, 2, 1],
            1 => [3, 3, 1],
            _ => [1, 1, 1]
        };

        foreach (var pm in partyMembers)
        {
            var cards = await cardRepository.GetByClassIdAsync(pm.ClassId, cancellationToken);
            var topCards = cards.Take(3).ToList();

            for (var i = 0; i < topCards.Count && i < copyCounts.Length; i++)
            {
                var card = topCards[i];
                var copies = copyCounts[i];

                for (var j = 0; j < copies; j++)
                {
                    deck.Add(Deck.Create(
                        pm.PlayerId,
                        card.Id,
                        pm.Id,
                        pm.Color,
                        card.ManaCost,
                        card.Attack,
                        card.MagicAttack,
                        card.Armor,
                        card.MagicArmor,
                        card.Mana,
                        card.Strength,
                        card.Defense,
                        card.Draw,
                        card.EffectMultiplier,
                        card.Heal,
                        card.Poison,
                        card.LifeCost,
                        card.PlayerTargets,
                        card.EnemyTargets
                    ));
                }
            }
        }

        // Debug override: add extra cards to the first party member's deck for testing.
        // Leave DebugCardIds empty for normal deck building.
        if (DebugCardIds.Length > 0 && partyMembers.Count > 0)
        {
            var firstPm = partyMembers[0];
            foreach (var dm in DebugCardIds)
            {
                var debugCards = await cardRepository.GetByIdsAsync([dm], cancellationToken);
                deck.AddRange(debugCards.Select(c => Deck.Create(
                    firstPm.PlayerId,
                    c.Id,
                    firstPm.Id,
                    firstPm.Color,
                    c.ManaCost,
                    c.Attack,
                    c.MagicAttack,
                    c.Armor,
                    c.MagicArmor,
                    c.Mana,
                    c.Strength,
                    c.Defense,
                    c.Draw,
                    c.EffectMultiplier,
                    c.Heal,
                    c.Poison,
                    c.LifeCost,
                    c.PlayerTargets,
                    c.EnemyTargets
                )));
            }

            
        }

        return deck;
    }
}
