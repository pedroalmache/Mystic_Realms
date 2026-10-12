using Microsoft.AspNetCore.Identity;
using MysticRealms.Domain.CharacterClasses;
using MysticRealms.Domain.Players;
using MysticRealms.Domain.UserCharacters;

namespace MysticRealms.Domain.Identity;

public class ApplicationUser : IdentityUser
{
    public string GamerTag { get; set; } = string.Empty;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Player> Players { get; set; } = new List<Player>();
    public ICollection<UserCharacter> Characters { get; set; } = new List<UserCharacter>();
}