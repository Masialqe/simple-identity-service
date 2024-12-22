using Serilog.Context;

namespace IdentityApp.Middleware
{
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
