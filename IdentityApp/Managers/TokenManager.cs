using IdentityApp.Users.Infrastructure.Interfaces;
using IdentityApp.Managers.Interrfaces;
using IdentityApp.Common.Exceptions;
using IdentityApp.Users.Models;

namespace IdentityApp.Managers
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
