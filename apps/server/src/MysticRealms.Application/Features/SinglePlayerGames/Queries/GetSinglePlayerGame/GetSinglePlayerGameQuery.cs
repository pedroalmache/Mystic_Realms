using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetSinglePlayerGame;

public record GetSinglePlayerGameQuery(int GameId, string UserId) : IRequest<GetSinglePlayerGameResult?>;

public record GetSinglePlayerGameResult(
    int GameId,
    int Level,
    string Path,
    List<GameAction>? ActionLog,
    RoomDto? Room);
