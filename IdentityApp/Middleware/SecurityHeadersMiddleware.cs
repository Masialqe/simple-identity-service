using Microsoft.Extensions.Primitives;

namespace IdentityApp.Middleware
{
    /// <summary>
    /// Adds security headers to API responses.
    /// </summary>
    /// <param name="_next"></param>
    public sealed class SecurityHeadersMiddleware(
        RequestDelegate _next)
    {
        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("X-Content-Type-Options", new StringValues("nosniff"));
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", new StringValues("none"));
            context.Response.Headers.Add("Content-Security-Policy", new StringValues(
                "default-src 'none'; " +
                "connect-src 'self'; " +
                "frame-ancestors 'none';"
            ));

            context.Response.Headers.Remove("Server");

            return _next(context);
        }
    }
}
