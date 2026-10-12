using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetActionLog;

public record GetActionLogQuery(int GameId, string UserId) : IRequest<List<GameAction>?>;
