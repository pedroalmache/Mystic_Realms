using MediatR;
using MysticRealms.Application.Interfaces;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetActiveSinglePlayerGame;

public class GetActiveSinglePlayerGameQueryHandler
    : IRequestHandler<GetActiveSinglePlayerGameQuery, ActiveSinglePlayerGameResult?>
{
    private readonly IGameRepository _gameRepository;

    public GetActiveSinglePlayerGameQueryHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<ActiveSinglePlayerGameResult?> Handle(
        GetActiveSinglePlayerGameQuery request,
        CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetActiveSinglePlayerGameByUserIdAsync(
            request.UserId, cancellationToken);

        if (game is null)
            return null;

        return new ActiveSinglePlayerGameResult(game.Id, game.Code);
    }
}
