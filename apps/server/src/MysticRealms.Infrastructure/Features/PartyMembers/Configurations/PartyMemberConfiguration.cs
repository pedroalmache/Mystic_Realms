using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MysticRealms.Domain.PartyMembers;

namespace MysticRealms.Infrastructure.Features.PartyMembers.Configurations;

public class PartyMemberConfiguration: IEntityTypeConfiguration<PartyMember>
{
    public void Configure(EntityTypeBuilder<PartyMember> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(p => p.InstanceId)
            .IsRequired();

        builder.HasIndex(p => p.InstanceId)
            .IsUnique();

        builder.HasOne(p => p.Player)
            .WithMany(p => p.PartyMembers)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Class)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}