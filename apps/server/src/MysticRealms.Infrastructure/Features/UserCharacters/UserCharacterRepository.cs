using Microsoft.EntityFrameworkCore;
using MysticRealms.Application.Features.Characters.Queries.GetAllCharacters;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.UserCharacters;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.UserCharacters;

public class UserCharacterRepository(ApplicationDbContext context) : IUserCharacterRepository
{
    public async Task<UserCharacter> AddAsync(string userId, int characterId, CancellationToken cancellationToken = default)
    {
        var userCharacter = UserCharacter.Create(userId, characterId);
        context.UserCharacters.Add(userCharacter);
        await context.SaveChangesAsync(cancellationToken);
        return userCharacter;
    }

    public async Task<List<UserCharacter>> Get(string userId, CancellationToken cancellationToken = default)
    {
        return await context.UserCharacters
            .Where(w => w.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}