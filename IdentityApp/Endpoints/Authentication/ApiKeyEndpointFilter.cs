using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Common.Configuration;
using IdentityApp.Common.Exceptions;
using Microsoft.Extensions.Options;

namespace IdentityApp.Endpoints.Authentication
{
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

        private bool IsApiKeyValid(string? requestApiKey, string? applicationApiKey)
        {
            if(applicationApiKey == null)
                throw new ApplicationConfigurationException();

            return requestApiKey is not null && applicationApiKey.Equals(requestApiKey);
        }
    }
}
