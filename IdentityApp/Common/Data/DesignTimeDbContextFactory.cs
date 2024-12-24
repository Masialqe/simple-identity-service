using Microsoft.EntityFrameworkCore.Design;

namespace IdentityApp.Common.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddNpgsqlDbContext<IdentityDbContext>(connectionName: "identityDb");

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        }
    }
}
