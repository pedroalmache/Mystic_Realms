using MysticRealms.Domain.Dungeons;

namespace MysticRealms.Application.Interfaces;

public interface IDungeonRepository
{
    Task<Dungeon> AddAsync(Dungeon dungeon, CancellationToken cancellationToken = default);
    Task UpdateAsync(Dungeon dungeon, CancellationToken cancellationToken = default);
}
