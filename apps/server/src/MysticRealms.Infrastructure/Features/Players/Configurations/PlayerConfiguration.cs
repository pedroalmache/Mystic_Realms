using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MysticRealms.Domain.Players;

namespace MysticRealms.Infrastructure.Features.Players.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(p => p.User)
            .WithMany(u => u.Players)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Game)
            .WithMany(g => g.Players)
            .HasForeignKey(d => d.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}