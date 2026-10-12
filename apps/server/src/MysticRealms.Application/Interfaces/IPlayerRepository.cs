using MysticRealms.Domain.Players;

namespace MysticRealms.Application.Interfaces;

public interface IPlayerRepository
{
    Task<Player> AddAsync(Player player, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
