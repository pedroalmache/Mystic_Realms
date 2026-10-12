using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetDrawPile;

public class GetDrawPileQueryHandler : IRequestHandler<GetDrawPileQuery, GetDrawPileResult?>
{
    private readonly IGameRepository _gameRepository;

    public GetDrawPileQueryHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<GetDrawPileResult?> Handle(
        GetDrawPileQuery request,
        CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);

        if (string.IsNullOrEmpty(game?.Save))
            return null;

        var save = SavedGameDto.FromJson(game.Save);

        var userPlayer = save?.Players.FirstOrDefault(p => p.UserId == request.UserId);

        if (userPlayer is null)
            return null;

        var cards = userPlayer.DrawPile
            .OrderBy(c => c.Name)
            .ToList();

        return new GetDrawPileResult(cards);
    }
}
