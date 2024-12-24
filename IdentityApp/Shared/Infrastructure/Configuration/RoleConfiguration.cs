using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Shared.Infrastructure.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("RoleId");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("RoleName");

            builder.HasMany(x => x.Users).WithMany(y => y.Roles);
        }
    }
}
