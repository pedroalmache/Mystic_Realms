using MysticRealms.Domain.Enemies;

namespace MysticRealms.Application.Interfaces;

public interface IEnemyRepository
{
    Task<IList<Enemy>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}
