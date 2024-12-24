using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Common.Data;

namespace IdentityApp.Extensions
{
    public static class DatabaseBuilderExtension
    {
        public static async Task ConfigureDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            await EnsureDatabaseCreated(context);
            await ExecuteDatabaseMigrations(context);
        }

        public static async Task EnsureDatabaseCreated(IdentityDbContext context)
        {
            var dbCreator = context.GetService<IRelationalDatabaseCreator>();

            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                if (!await dbCreator.ExistsAsync())
                    await dbCreator.CreateAsync();
            });
        }

        public static async Task ExecuteDatabaseMigrations(IdentityDbContext context)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                //await using var transaction = await context.Database.BeginTransactionAsync();
                await context.Database.MigrateAsync();
                //await transaction.CommitAsync();
            });
        }
    }
}
