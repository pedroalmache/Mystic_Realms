using MysticRealms.Domain.CharacterClasses;
using MysticRealms.Domain.Identity;

namespace MysticRealms.Domain.UserCharacters;

public class UserCharacter
{
    public int Id { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public ApplicationUser User { get; private set; } = null!;
    
    public int CharacterId { get; private set; }
    public CharacterClass Character { get; private set; } = null!;
    
    public bool IsUnlocked { get; private set; } = false;

    private UserCharacter()
    {
    }

    private UserCharacter(string userId, int characterId)
    {
        UserId = userId;
        CharacterId = characterId;
        IsUnlocked = true;
    }

    public static UserCharacter Create(string userId, int characterId)
    {
        return new UserCharacter(userId, characterId);
    }
}