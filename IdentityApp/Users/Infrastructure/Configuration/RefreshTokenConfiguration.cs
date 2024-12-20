using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Users.Models;

namespace IdentityApp.Users.Infrastructure.Configuration
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Token).HasMaxLength(256);

            builder.HasIndex(x => x.Token).IsUnique();

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
