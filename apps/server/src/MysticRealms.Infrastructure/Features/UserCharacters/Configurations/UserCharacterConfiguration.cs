using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MysticRealms.Domain.UserCharacters;

namespace MysticRealms.Infrastructure.Features.UserCharacters.Configurations;

public class UserCharacterConfiguration: IEntityTypeConfiguration<UserCharacter>
{
    public void Configure(EntityTypeBuilder<UserCharacter> builder)
    {
        builder.HasKey(userCharacter => userCharacter.Id);
        
        builder.HasOne(r => r.Character).WithMany().HasForeignKey(r => r.CharacterId);
        builder.HasOne(r => r.User)
            .WithMany(r => r.Characters)
            .HasForeignKey(r => r.UserId);
    }
}