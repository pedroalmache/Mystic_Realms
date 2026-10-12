using MediatR;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetActiveSinglePlayerGame;

public record GetActiveSinglePlayerGameQuery(string UserId) : IRequest<ActiveSinglePlayerGameResult?>;

public record ActiveSinglePlayerGameResult(int GameId, string Code);
