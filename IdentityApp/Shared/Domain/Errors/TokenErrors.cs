using IdentityApp.Shared.Abstractions.Errors;

namespace IdentityApp.Shared.Domain.Errors
{
    public static class TokenErrors
    {
        public static Error InvalidRefreshTokenError => Error.Unauthorized("Invalid refresh token.");
        public static Error AccessDeniedError => Error.Forbidden("Access denied. Need to re-authenticate using credentials.");
    }
}
