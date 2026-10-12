using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Domain.Games;

namespace MysticRealms.Infrastructure.Features.Games.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{

    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Code)
            .IsRequired()
            .HasMaxLength(16);

        builder.Property(g => g.Save)
            .IsRequired(false);
        
        builder.HasOne(d => d.Dungeon)
            .WithOne(p => p.Game)
            .HasForeignKey<Dungeon>(d => d.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}