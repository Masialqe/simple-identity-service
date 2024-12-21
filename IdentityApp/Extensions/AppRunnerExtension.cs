﻿namespace IdentityApp.Extensions
{
    public static class AppRunnerExtension
    {
        public static WebApplication RunAplication(this WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<WebApplication>>();
            
            try
            {
                app.Run();
                logger.LogInformation("App has been started successfully. {TimeStamp}", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to start application due to exception. {ExceptionMessage} - {Exception}", ex.Message, ex);
            }

            return app;
        }
    }
}