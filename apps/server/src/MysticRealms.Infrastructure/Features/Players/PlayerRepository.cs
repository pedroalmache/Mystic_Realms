using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Players;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.Players;

public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Player> AddAsync(Player player, CancellationToken cancellationToken = default)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync(cancellationToken);
        return player;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
