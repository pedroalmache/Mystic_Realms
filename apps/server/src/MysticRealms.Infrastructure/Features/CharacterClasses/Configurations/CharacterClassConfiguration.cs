using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MysticRealms.Domain.CharacterClasses;
using MysticRealms.Infrastructure.SeedData;

namespace MysticRealms.Infrastructure.Features.CharacterClasses.Configurations;

public class CharacterClassConfiguration : IEntityTypeConfiguration<CharacterClass>
{
    public void Configure(EntityTypeBuilder<CharacterClass> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasData(CharactersSeedData.Get());
    }
}
