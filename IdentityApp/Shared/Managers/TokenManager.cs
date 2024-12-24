using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Managers.Interrfaces;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Shared.Exceptions;

namespace IdentityApp.Shared.Managers
{
    public sealed class TokenManager(
        IJwtManager jwtManager,
        IRefreshTokenManager refreshTokenManager,
        ITokenRepository tokenRepository) : ITokenManager
    {
        public async Task<(string, string)> GenerateAccessTokenWithRefreshToken(User? user)
        {
            if (user is null) throw new ApplicationProcessException();

            var accessToken = GenerateAccessToken(user);
            var refreshToken = await refreshTokenManager.CreateRefreshToken(user);

            return (accessToken, refreshToken);
        }

        public string GenerateAccessToken(User? user)
        {
            if (user is null) throw new ApplicationProcessException();

            var accessToken = jwtManager.CreateAccessToken(user);
            return accessToken;
        }

        public async Task<RefreshToken> RenewRefreshTokenAsync(RefreshToken? tokenToRefresh)
        {
            if (tokenToRefresh is null) throw new ApplicationProcessException();

            tokenToRefresh.Token = refreshTokenManager.GenerateRefreshTokenValue();
            tokenToRefresh.ExpiresOnUtc = refreshTokenManager.GenerateTokenExpirationDate();

            await tokenRepository.UpdateRefreshTokenAsync(tokenToRefresh);

            return tokenToRefresh;
        }
    }
}
