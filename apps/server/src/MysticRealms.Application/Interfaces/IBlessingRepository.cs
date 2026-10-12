using MysticRealms.Domain.Blessings;
using MysticRealms.Domain.Dungeons;

namespace MysticRealms.Application.Interfaces;

public interface IBlessingRepository
{
    Task<IList<Blessing>> GetByTypeAsync(RewardType type, CancellationToken cancellationToken = default);
}
