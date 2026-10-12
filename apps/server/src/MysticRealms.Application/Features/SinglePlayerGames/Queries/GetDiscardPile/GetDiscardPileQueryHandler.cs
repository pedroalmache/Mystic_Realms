using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetDiscardPile;

public class GetDiscardPileQueryHandler : IRequestHandler<GetDiscardPileQuery, GetDiscardPileResult?>
{
    private readonly IGameRepository _gameRepository;

    public GetDiscardPileQueryHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<GetDiscardPileResult?> Handle(
        GetDiscardPileQuery request,
        CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);

        if (string.IsNullOrEmpty(game?.Save))
            return null;

        var save = SavedGameDto.FromJson(game.Save);

        var userPlayer = save?.Players.FirstOrDefault(p => p.UserId == request.UserId);

        if (userPlayer is null)
            return null;

        var cards = userPlayer.DiscardPile
            .ToList();

        return new GetDiscardPileResult(cards);
    }
}
