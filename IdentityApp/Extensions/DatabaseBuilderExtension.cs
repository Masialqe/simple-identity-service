using Microsoft.EntityFrameworkCore.Infrastructure;
using IdentityApp.Shared.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring and managing the database lifecycle during application startup.
    /// </summary>
    public static class DatabaseBuilderExtension
    {
        /// <summary>
        /// Configures the database by ensuring it is created and up-to-date with the latest migrations.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> instance to configure the database for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ConfigureDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            await EnsureDatabaseCreated(context);
            await ExecuteDatabaseMigrations(context);
        }

        /// <summary>
        /// Ensures that the database is created if it does not already exist.
        /// </summary>
        /// <param name="context">The <see cref="IdentityDbContext"/> instance used to check and create the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Applies pending database migrations to ensure the database schema is up-to-date.
        /// </summary>
        /// <param name="context">The <see cref="IdentityDbContext"/> instance used to apply the migrations.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ExecuteDatabaseMigrations(IdentityDbContext context)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await context.Database.MigrateAsync();
            });
        }
    }

}
