using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Shared.Infrastructure.Data;

namespace IdentityApp.Extensions
{
    public static class DatabaseBuilderExtension
    {
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
                await using var transaction = await context.Database.BeginTransactionAsync();
                await context.Database.MigrateAsync();
                await transaction.CommitAsync();
            });
        }
    }
}
