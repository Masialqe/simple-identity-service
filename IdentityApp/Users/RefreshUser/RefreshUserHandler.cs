using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Abstractions.ApiResults;
using IdentityApp.Shared.Managers.Interrfaces;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Endpoints.Responses;

namespace IdentityApp.Users.RefreshUser
{
    public record RefreshUserRequest(string refreshToken);
    public sealed class RefreshUserHandler(
        ITokenRepository tokenRepository,
        IRefreshUserValidator refreshTokenValidator,
        IHttpContextAccessor httpContextAccessor,
        ITokenManager tokenManager, ILogger<RefreshUserHandler> logger) : IRefreshUserHandler
    {
        public async Task<IResult> Handle(RefreshUserRequest request)
        {
            var refreshTokenState = await tokenRepository.GetRefreshTokenByValueAsync(
                request.refreshToken, httpContextAccessor.HttpContext?.RequestAborted ?? default);

            var refreshTokenValidationResult = await refreshTokenValidator.ValidateAsync(refreshTokenState, request);

            if (refreshTokenValidationResult.IsFailure)
                return refreshTokenValidationResult.ToProblemDetails();

            (var accessToken, var renevedRefreshToken) = await GenerateAccessTokensAsync(refreshTokenState);

            var response = new RefreshUserResponse(accessToken, renevedRefreshToken);
            logger.LogInformation("User {UserLogin} has been logged in.", refreshTokenState?.User?.Login);

            return ApiResponseFactory.Ok(response);
        }

        private async Task<(string, string)> GenerateAccessTokensAsync(RefreshToken? refreshTokenState)
        {
            var accessToken = tokenManager.GenerateAccessToken(refreshTokenState?.User);
            var renevedRefreshToken = await tokenManager.RenewRefreshTokenAsync(refreshTokenState);

            return (accessToken, renevedRefreshToken.Token);
        }
    }
}
