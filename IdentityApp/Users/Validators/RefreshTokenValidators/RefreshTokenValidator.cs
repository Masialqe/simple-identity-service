using IdentityApp.Users.Infrastructure.Interfaces;
using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Users.Models;
using IdentityApp.Users.Errors;

namespace IdentityApp.Users.Validators.RefreshTokenValidators
{
    public sealed class RefreshTokenValidator(
        ITokenRepository tokenRepository,
        IHttpContextAccessor httpContextAccessor) : IRefreshTokenValidator
    {
        public async Task<Result> ValidateAsync(
            RefreshToken? refreshToken, RefreshUserRequest request)
        {
            if (refreshToken is null)
                return TokenErrors.InvalidRefreshTokenError;

            if (!IsTokenValid(refreshToken))
                return TokenErrors.InvalidRefreshTokenError;

            if (await IsReauthenticationRequired(refreshToken, request))
            {
                await tokenRepository.DeleteRefreshTokensPerUserAsync(refreshToken.UserId);
                return TokenErrors.AccessDeniedError;
            }

            return Result.Success();
        }

        private bool IsTokenValid(RefreshToken refreshToken)
            => refreshToken.ExpiresOnUtc > DateTime.UtcNow && refreshToken.User is not null;

        private async Task<bool> IsReauthenticationRequired(
            RefreshToken refreshToken, RefreshUserRequest request)
        {
            var newestToken = await tokenRepository.GetNewestRefreshTokenPerUserAsync(refreshToken.UserId);
            var currentUserAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            if (newestToken is null)
                return true;

            return newestToken != request.refreshToken
                || refreshToken.User?.SourceAddres != currentUserAddress;
        }
    }
}