namespace IdentityApp.Extensions
{
    public static class AppRunnerExtension
    {
        /// <summary>
        /// Configures and runs the web application, logging any startup errors.
        /// </summary>
        /// <param name="app">The WebApplication instance to configure and run.</param>
        /// <returns>The configured and running WebApplication instance.</returns>
        public async static Task<WebApplication> RunApplication(this WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<WebApplication>>();

            try
            {
                await app.ConfigureDatabaseAsync();
                app.Run();
                logger.LogInformation("Application started and running successfully. {TimeStamp}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                logger.LogError("Application failed to start due to an exception. {ExceptionMessage} - {Exception}", ex.Message, ex);
            }

            return app;
        }

    }
}