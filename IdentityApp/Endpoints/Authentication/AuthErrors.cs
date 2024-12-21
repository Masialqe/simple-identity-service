using IdentityApp.Common.Abstractions.Errors;

namespace IdentityApp.Endpoints.Authentication
{
    public static class AuthErrors
    {
        public static Error InvalidApiKeyError => Error.Unauthorized("Invalid api key.");
    }
}
