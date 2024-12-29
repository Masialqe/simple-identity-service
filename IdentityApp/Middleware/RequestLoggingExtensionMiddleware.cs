using Serilog.Context;

namespace IdentityApp.Middleware
{
    /// <summary>
    /// Extends Serilog request loggin by including CorrelationId.
    /// </summary>
    /// <param name="_next"></param>
    public class RequestLoggingExtensionMiddleware(
        RequestDelegate _next)
    {
        public Task InvokeAsync(HttpContext context)
        {
            using(LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
            {
                return _next(context);
            }
        }
    }
}
