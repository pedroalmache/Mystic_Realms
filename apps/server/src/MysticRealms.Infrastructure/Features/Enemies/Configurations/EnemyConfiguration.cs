using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MysticRealms.Domain.Enemies;
using MysticRealms.Infrastructure.SeedData;

namespace MysticRealms.Infrastructure.Features.Enemies.Configurations;

public class EnemyConfiguration : IEntityTypeConfiguration<Enemy>
{
    public void Configure(EntityTypeBuilder<Enemy> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Guid)
            .IsRequired();

        builder.HasIndex(e => e.Guid)
            .IsUnique();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(EnemiesSeedData.Get());
    }
}
