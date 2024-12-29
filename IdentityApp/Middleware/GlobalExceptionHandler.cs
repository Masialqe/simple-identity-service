using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IdentityApp.Middleware
{
    /// <summary>
    /// Handles global exceptions by logging details and returning appropriate problem details responses.
    /// </summary>
    /// <param name="problemDetailsService">Service for generating and writing problem details responses.</param>
    /// <param name="logger">Logger for recording exception details and diagnostic information.</param>
    public sealed class GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;

            logger.LogError(
                "Error occurred while executing endpoint. {Message}, {RequestId}, {RequestMethod}, {StatusCode}, {RequestSource}",
                exception.Message,
                httpContext.TraceIdentifier,
                httpContext.Request.Method,
                httpContext.Response.StatusCode,
                httpContext.Connection.RemoteIpAddress);

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().Name,
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An error occurred while processing your request.",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    Extensions = new Dictionary<string, object?>
                {
                    { "requestId", httpContext.TraceIdentifier },
                    { "traceId", activity?.Id },
                    { "timestamp", DateTime.Now }
                }
                }
            });
        }
    }

}
