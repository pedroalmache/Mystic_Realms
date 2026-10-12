using MysticRealms.Domain.Dungeons;

namespace MysticRealms.Application.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByDungeonIdAsync(int dungeonId, CancellationToken cancellationToken = default);
    Task<Room> AddAsync(Room room, CancellationToken cancellationToken = default);
    Task UpdateAsync(Room room, CancellationToken cancellationToken = default);
}
