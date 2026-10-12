using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MysticRealms.Domain.Decks;

namespace MysticRealms.Infrastructure.Features.Decks.Configurations;

public class DeckConfiguration: IEntityTypeConfiguration<Deck>
{
    public void Configure(EntityTypeBuilder<Deck> builder)
    {
        builder.Property(d => d.MagicAttack).HasDefaultValue(0);
        builder.Property(d => d.MagicArmor).HasDefaultValue(0);

        builder.HasOne(d => d.Player)
            .WithMany(p => p.Deck)
            .HasForeignKey(d => d.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(d => d.Card)
            .WithMany()
            .HasForeignKey(d => d.CardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}