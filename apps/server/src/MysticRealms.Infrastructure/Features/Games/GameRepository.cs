using Microsoft.EntityFrameworkCore;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Games;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.Games;

public class GameRepository : IGameRepository
{
    private readonly ApplicationDbContext _context;

    public GameRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Game?> GetActiveSinglePlayerGameByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Games
            .Include(g => g.Players)
            .FirstOrDefaultAsync(
                g => g.IsActive && !g.IsMultiplayer && g.Players.Any(p => p.UserId == userId),
                cancellationToken);
    }

    public async Task<Game?> GetByIdAsync(int gameId, CancellationToken cancellationToken = default)
    {
        return await _context.Games
            .Include(g => g.Dungeon)
            .Include(g => g.Players)
                .ThenInclude(p => p.Deck)
                    .ThenInclude(d => d.Card)
            .Include(g => g.Players)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(g => g.Id == gameId, cancellationToken);
    }

    public async Task DeactivateAllSinglePlayerGamesForUserAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var games = await _context.Games
            .Include(g => g.Players)
            .Where(g => g.IsActive && !g.IsMultiplayer && g.Players.Any(p => p.UserId == userId))
            .ToListAsync(cancellationToken);

        foreach (var game in games)
            game.Deactivate();

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Game> AddAsync(Game game, CancellationToken cancellationToken = default)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync(cancellationToken);
        return game;
    }

    public async Task UpdateAsync(Game game, CancellationToken cancellationToken = default)
    {
        _context.Games.Update(game);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
