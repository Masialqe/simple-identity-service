using IdentityApp.Shared.Abstractions.ApiResults;
using IdentityApp.Shared.Exceptions;
using Microsoft.Extensions.Options;
using IdentityApp.Configuration;

namespace IdentityApp.Endpoints.Authentication
{
    /// <summary>
    /// An endpoint filter to validate API key headers for requests.
    /// </summary>
    public sealed class ApiKeyEndpointFilter(
        IOptions<SecretsOptions> options) : IEndpointFilter
    {
        private const string ApiKeyHeaderName = "X-Api-Key";

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var requestApiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];
            var apiKey = options.Value.ApiKey;

            if (!IsApiKeyValid(requestApiKey, apiKey))
                return Result.Failure(AuthErrors.InvalidApiKeyError).ToProblemDetails();

            return await next(context);
        }

        /// <summary>
        /// Determines whether the provided API key matches the configured application API key.
        /// </summary>
        /// <param name="requestApiKey">The API key from the request headers.</param>
        /// <param name="applicationApiKey">The configured application API key.</param>
        /// <returns>True if the keys match; otherwise, false.</returns>
        private bool IsApiKeyValid(string? requestApiKey, string? applicationApiKey)
        {
            if (applicationApiKey == null)
                throw new ApplicationConfigurationException();

            return requestApiKey is not null && applicationApiKey.Equals(requestApiKey);
        }
    }
}
