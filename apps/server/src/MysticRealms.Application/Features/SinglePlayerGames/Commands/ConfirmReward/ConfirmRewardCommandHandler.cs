using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Features.SinglePlayerGames.Queries.GetRoomRewards;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Decks;
using MysticRealms.Domain.Dungeons;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands.ConfirmReward;

public class ConfirmRewardCommandHandler(
    IMediator mediator,
    IGameRepository gameRepository,
    IRoomRepository roomRepository,
    IPartyMemberRepository partyMemberRepository,
    ICardRepository cardRepository,
    IDeckRepository deckRepository) : IRequestHandler<ConfirmRewardCommand>
{
    public async Task Handle(ConfirmRewardCommand request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(request.GameId, cancellationToken);
        if (game is null)
            throw new InvalidOperationException("Game not found.");

        var player = game.Players.FirstOrDefault(p => p.UserId == request.UserId);
        if (player is null)
            throw new InvalidOperationException("Player not found in game.");

        var room = await roomRepository.GetByDungeonIdAsync(game.Dungeon.Id, cancellationToken);

        if (room?.Type != RoomType.EncounterReward)
            throw new InvalidOperationException("Rewards can only be confirmed in an encounter reward room.");

        var rewardResult = await mediator.Send(new GetRoomRewardsQuery(request.GameId, string.Empty), cancellationToken)
            ?? throw new InvalidOperationException("Failed to retrieve room rewards.");

        var goldByPartyMemberId = rewardResult.Players
            .SelectMany(p => p.PartyMembers)
            .ToDictionary(pm => pm.PartyMemberId, pm => pm.Gold);

        var partyMembers = await partyMemberRepository.GetByGameIdAsync(request.GameId, cancellationToken);
        foreach (var partyMember in partyMembers)
        {
            if (goldByPartyMemberId.TryGetValue(partyMember.Id, out var gold))
                partyMember.AddGold(gold);
        }
        await partyMemberRepository.UpdateRangeAsync(partyMembers, cancellationToken);

        var playerPartyMembersById = partyMembers
            .Where(pm => pm.PlayerId == player.Id)
            .ToDictionary(pm => pm.Id);

        var cardsByClassId = new Dictionary<int, Dictionary<int, Domain.Cards.Card>>();
        var rewardDeckEntries = new List<Deck>();
        foreach (var selection in request.Cards)
        {
            if (!playerPartyMembersById.TryGetValue(selection.PartyMemberId, out var partyMember))
                throw new InvalidOperationException($"Party member {selection.PartyMemberId} does not belong to the current player.");

            if (!cardsByClassId.TryGetValue(partyMember.ClassId, out var cardsForClass))
            {
                cardsForClass = (await cardRepository.GetByClassIdAsync(partyMember.ClassId, cancellationToken))
                    .ToDictionary(c => c.Id);
                cardsByClassId[partyMember.ClassId] = cardsForClass;
            }

            if (!cardsForClass.TryGetValue(selection.CardId, out var card))
                throw new InvalidOperationException($"Card {selection.CardId} is not valid for party member {selection.PartyMemberId}.");

            var deckCard = Deck.Create(
                player.Id,
                card.Id,
                partyMember.Id,
                partyMember.Color,
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
                card.EnemyTargets);

            rewardDeckEntries.Add(deckCard);
        }
        await deckRepository.AddRangeAsync(rewardDeckEntries, cancellationToken);

        room.Update(RoomType.RoomSelection, RewardType.None, room.Seed, room.EnemyIds, room.BackgroundId);
        await roomRepository.UpdateAsync(room, cancellationToken);

        var save = SavedGameDto.FromJson(game.Save)
            ?? throw new InvalidOperationException("Failed to load game save.");

        if (save.CurrentRoom is null)
            save.CurrentRoom = new RoomDto(room.Type, room.Reward, room.Seed, room.BackgroundId);
        else
        {
            save.CurrentRoom.Type = RoomType.RoomSelection;
            save.CurrentRoom.Reward = RewardType.None;
        }

        game.SetSave(save.ToJson());
        await gameRepository.UpdateAsync(game, cancellationToken);
    }
}
