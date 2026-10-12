using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MysticRealms.Domain.Dungeons;
using System.Text.Json;

namespace MysticRealms.Infrastructure.Features.Rooms.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.Property(r => r.Seed)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(r => r.EnemyIds)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<int[]>(v, (JsonSerializerOptions?)null) ?? Array.Empty<int>()
            )
            .HasColumnType("nvarchar(max)");

        builder.HasOne(r => r.Dungeon)
            .WithOne()
            .HasForeignKey<Room>(r => r.DungeonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
