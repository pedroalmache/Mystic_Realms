using MysticRealms.Domain.Games;

namespace MysticRealms.Application.Interfaces;

public interface IGameRepository
{
    Task<Game?> GetActiveSinglePlayerGameByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Game?> GetByIdAsync(int gameId, CancellationToken cancellationToken = default);
    Task<Game> AddAsync(Game game, CancellationToken cancellationToken = default);
    Task UpdateAsync(Game game, CancellationToken cancellationToken = default);
    Task DeactivateAllSinglePlayerGamesForUserAsync(string userId, CancellationToken cancellationToken = default);
}
