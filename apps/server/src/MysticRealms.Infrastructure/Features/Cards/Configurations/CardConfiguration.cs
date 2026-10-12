using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MysticRealms.Domain.Cards;
using MysticRealms.Infrastructure.SeedData;

namespace MysticRealms.Infrastructure.Features.Cards.Configurations;

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.MagicAttack).HasDefaultValue(0);
        builder.Property(c => c.MagicArmor).HasDefaultValue(0);
        builder.Property(c => c.AscendedMagicAttack).HasDefaultValue(0);
        builder.Property(c => c.AscendedMagicArmor).HasDefaultValue(0);

        builder.HasData(WarriorCardsSeedData.Get());
        builder.HasData(AssassinCardsSeedData.Get());
        builder.HasData(RangerCardsSeedData.Get());
        builder.HasData(PaladinCardsSeedData.Get());
        builder.HasData(WhiteMageCardsSeedData.Get());
        builder.HasData(BlackMageCardsSeedData.Get());
        builder.HasData(ElementalistCardsSeedData.Get());
        builder.HasData(DruidCardsSeedData.Get());
        builder.HasData(BarbarianCardsSeedData.Get());
        builder.HasData(EngineerCardsSeedData.Get());
    }
}
