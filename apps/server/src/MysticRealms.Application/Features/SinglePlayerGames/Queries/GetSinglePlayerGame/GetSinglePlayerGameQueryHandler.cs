using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetSinglePlayerGame;

public class GetSinglePlayerGameQueryHandler(
    IGameRepository gameRepository,
    IRoomRepository roomRepository)
    : IRequestHandler<GetSinglePlayerGameQuery, GetSinglePlayerGameResult?>
{
    public async Task<GetSinglePlayerGameResult?> Handle(
        GetSinglePlayerGameQuery request,
        CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            return null;

        var actionLogs = new List<GameAction>();
        if (!string.IsNullOrEmpty(game.Save))
        {
            var save = SavedGameDto.FromJson(game.Save);
            actionLogs = save?.ActionLog ?? [];
        }

        var room = await roomRepository.GetByDungeonIdAsync(game.Dungeon.Id, cancellationToken);
        var roomDto = room is null
            ? null
            : new RoomDto(room.Type, room.Reward, room.Seed, room.BackgroundId);

        return new GetSinglePlayerGameResult(
            game.Id,
            game.Dungeon.Level,
            game.Dungeon.Path,
            actionLogs.Count == 0 ? null : actionLogs,
            roomDto
        );
    }
}
