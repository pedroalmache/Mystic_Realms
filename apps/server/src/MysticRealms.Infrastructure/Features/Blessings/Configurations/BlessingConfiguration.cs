using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MysticRealms.Domain.Blessings;
using MysticRealms.Infrastructure.SeedData;

namespace MysticRealms.Infrastructure.Features.Blessings.Configurations;

public class BlessingConfiguration : IEntityTypeConfiguration<Blessing>
{
    public void Configure(EntityTypeBuilder<Blessing> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Level_1_Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(b => b.Level_2_Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(b => b.Level_3_Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(b => b.Level_4_Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasData(BlessingsSeedData.Get());
    }
}
