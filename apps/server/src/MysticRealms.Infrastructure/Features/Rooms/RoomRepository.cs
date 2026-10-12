using Microsoft.EntityFrameworkCore;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.Rooms;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _context;

    public RoomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Room?> GetByDungeonIdAsync(int dungeonId, CancellationToken cancellationToken = default)
    {
        return await _context.Rooms
            .FirstOrDefaultAsync(r => r.DungeonId == dungeonId, cancellationToken);
    }

    public async Task<Room> AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync(cancellationToken);
        return room;
    }

    public async Task UpdateAsync(Room room, CancellationToken cancellationToken = default)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
