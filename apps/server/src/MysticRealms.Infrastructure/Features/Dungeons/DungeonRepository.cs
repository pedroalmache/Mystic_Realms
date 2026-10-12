using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.Dungeons;

public class DungeonRepository : IDungeonRepository
{
    private readonly ApplicationDbContext _context;

    public DungeonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Dungeon> AddAsync(Dungeon dungeon, CancellationToken cancellationToken = default)
    {
        _context.Dungeons.Add(dungeon);
        await _context.SaveChangesAsync(cancellationToken);
        return dungeon;
    }

    public async Task UpdateAsync(Dungeon dungeon, CancellationToken cancellationToken = default)
    {
        _context.Dungeons.Update(dungeon);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
