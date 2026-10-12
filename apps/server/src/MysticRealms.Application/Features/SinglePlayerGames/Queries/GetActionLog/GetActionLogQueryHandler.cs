using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetActionLog;

public class GetActionLogQueryHandler : IRequestHandler<GetActionLogQuery, List<GameAction>?>
{
    private readonly IGameRepository _gameRepository;

    public GetActionLogQueryHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<List<GameAction>?> Handle(
        GetActionLogQuery request,
        CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);

        if (game is null)
            return null;

        if (string.IsNullOrEmpty(game.Save))
            return null;

        var save = SavedGameDto.FromJson(game.Save);
        if (save is null)
            return null;

        return save.ActionLog;
    }
}
