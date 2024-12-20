using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Common.Exceptions;

namespace IdentityApp.Endpoints.Authentication
{
    public sealed class ApiKeyEndpointFilter(
        IConfiguration configuration) : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var requestApiKey = context.HttpContext.Request.Headers[ApiKeyOptions.API_KEY_HEADER_NAME];
            var apiKey = configuration.GetValue<string>(ApiKeyOptions.API_KEY_VALUE);

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
