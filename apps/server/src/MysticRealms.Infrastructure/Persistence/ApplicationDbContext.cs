using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MysticRealms.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Domain.Identity;
using MysticRealms.Domain.Games;
using MysticRealms.Domain.PartyMembers;
using MysticRealms.Domain.Players;
using MysticRealms.Domain.Enemies;
using MysticRealms.Domain.Cards;
using MysticRealms.Domain.CharacterClasses;
using MysticRealms.Domain.Decks;
using MysticRealms.Domain.Blessings;
using MysticRealms.Domain.UserCharacters;
using MysticRealms.Infrastructure.Features.Cards.Configurations;
using MysticRealms.Infrastructure.Features.CharacterClasses.Configurations;
using MysticRealms.Infrastructure.Features.Enemies.Configurations;
using MysticRealms.Infrastructure.Features.Games.Configurations;
using MysticRealms.Infrastructure.Features.PartyMembers.Configurations;
using MysticRealms.Infrastructure.Features.Players.Configurations;
using MysticRealms.Infrastructure.Features.Blessings.Configurations;
using MysticRealms.Infrastructure.Features.Rooms.Configurations;

namespace MysticRealms.Infrastructure.Persistence;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<string>, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Dungeon> Dungeons => Set<Dungeon>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<CharacterClass> CharacterClasses => Set<CharacterClass>();
    public DbSet<PartyMember> PartyMembers => Set<PartyMember>();
    public DbSet<Enemy> Enemies => Set<Enemy>();
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Deck> Decks => Set<Deck>();
    public DbSet<Blessing> Blessings => Set<Blessing>();
    public DbSet<UserCharacter> UserCharacters => Set<UserCharacter>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.GamerTag)
                  .HasMaxLength(50)
                  .IsRequired();
        });
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
        builder.ApplyConfiguration(new GameConfiguration());
        builder.ApplyConfiguration(new PlayerConfiguration());
        builder.ApplyConfiguration(new PartyMemberConfiguration());
        builder.ApplyConfiguration(new CharacterClassConfiguration());
        builder.ApplyConfiguration(new EnemyConfiguration());
        builder.ApplyConfiguration(new CardConfiguration());
        builder.ApplyConfiguration(new BlessingConfiguration());
        builder.ApplyConfiguration(new RoomConfiguration());
    }
}