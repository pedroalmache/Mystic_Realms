using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Dungeons;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetRoomOptions;

public class GetRoomOptionsQueryHandler(
    IGameRepository gameRepository,
    IRoomRepository roomRepository,
    DungeonGenerator dungeonGenerator)
    : IRequestHandler<GetRoomOptionsQuery, GetRoomOptionsResult?>
{
    public async Task<GetRoomOptionsResult?> Handle(GetRoomOptionsQuery request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(request.GameId, cancellationToken);
        if (game is null)
            return null;

        var room = await roomRepository.GetByDungeonIdAsync(game.Dungeon.Id, cancellationToken);
        if (room?.Type != RoomType.RoomSelection)
            return null;

        var nextLevel = game.Dungeon.Level + 1;

        var leftRoom = dungeonGenerator.GenerateRoom($"{game.Dungeon.Path}-L", nextLevel, game.Dungeon.Id);
        var rightRoom = dungeonGenerator.GenerateRoom($"{game.Dungeon.Path}-R", nextLevel, game.Dungeon.Id);

        var left = new RoomDto(leftRoom.Type, leftRoom.Reward, leftRoom.Seed, leftRoom.BackgroundId);
        var right = new RoomDto(rightRoom.Type, rightRoom.Reward, rightRoom.Seed, rightRoom.BackgroundId);

        return new GetRoomOptionsResult(left, right);
    }
}
