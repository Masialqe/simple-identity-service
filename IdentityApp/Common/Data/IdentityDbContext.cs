using Microsoft.EntityFrameworkCore;
using IdentityApp.Users.Models;

namespace IdentityApp.Common.Data
{
    public sealed class IdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
