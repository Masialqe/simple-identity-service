using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Users.Models;

namespace IdentityApp.Users.Infrastructure.Configuration
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("UserId");

            builder.Property(x => x.Login)
                .HasMaxLength(128)
                .IsRequired()
                .HasColumnName("UserLogin");
            builder.HasIndex(x => x.Login)
                .IsUnique();

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("UserPasswordHash");

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("UserIsActive");

            builder.Property(x => x.LoginAttemps)
                .HasDefaultValue(0)
                .HasColumnName("UserLoginAttemps");

            builder.Property(x => x.SourceAddres)
                .HasMaxLength(128)
                .HasColumnName("UserSourceAddres");

            builder.Property(x => x.BlockExpireOnUtc)
                .HasColumnName("UserBlockExpireOnUtc");
        }
    }
}
