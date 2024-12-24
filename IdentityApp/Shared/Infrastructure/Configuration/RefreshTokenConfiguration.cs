using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityApp.Shared.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Shared.Infrastructure.Configuration
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
