using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Abstractions.ApiResults;
using IdentityApp.Shared.Domain.Errors;
using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Users.RefreshUser
{
    /// <summary>
    /// Validates the refresh token and determines if reauthentication is required.
    /// </summary>
    public sealed class RefreshUserValidator(
        ITokenRepository tokenRepository,
        IHttpContextAccessor httpContextAccessor) : IRefreshUserValidator
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
            var currentUserAddress = GetCurrentUserAddressAsString();

            if (newestToken is null)
                return true;

            return newestToken != request.refreshToken
                || refreshToken.User?.SourceAddres != currentUserAddress;
        }

        private string GetCurrentUserAddressAsString()
            => httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()!;
    }
}