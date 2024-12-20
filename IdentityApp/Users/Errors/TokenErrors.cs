using IdentityApp.Common.Abstractions.Errors;

namespace IdentityApp.Users.Errors
{
    public static class TokenErrors
    {
        public static Error InvalidRefreshTokenError => Error.Unauthorize("Invalid refresh token.");
        public static Error AccessDeniedError => Error.Forbidden("Access denied. Need to re-authenticate using credentials.");
    }
}
