using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Managers.Interrfaces;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Shared.Exceptions;

namespace IdentityApp.Shared.Managers
{
    /// <summary>
    /// A service for managing access tokens and refresh tokens, including generating and renewing tokens.
    /// </summary>
    public sealed class TokenManager(
        IJwtManager jwtManager,
        IRefreshTokenManager refreshTokenManager,
        ITokenRepository tokenRepository) : ITokenManager
    {
        /// <summary>
        /// Generates an access token and a refresh token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the tokens are being generated.</param>
        /// <returns>A tuple containing the generated access token and refresh token.</returns>
        /// <exception cref="ApplicationProcessException">Thrown when the <paramref name="user"/> is null.</exception>
        public async Task<(string, string)> GenerateAccessTokenWithRefreshToken(User? user)
        {
            if (user is null) throw new ApplicationProcessException();

            var accessToken = GenerateAccessToken(user);
            var refreshToken = await refreshTokenManager.CreateRefreshToken(user);

            return (accessToken, refreshToken);
        }

        /// <summary>
        /// Generates an access token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the access token is being generated.</param>
        /// <returns>The generated access token.</returns>
        /// <exception cref="ApplicationProcessException">Thrown when the <paramref name="user"/> is null.</exception>
        public string GenerateAccessToken(User? user)
        {
            if (user is null) throw new ApplicationProcessException();

            var accessToken = jwtManager.CreateAccessToken(user);
            return accessToken;
        }

        /// <summary>
        /// Renews a refresh token by updating its value and expiration date.
        /// </summary>
        /// <param name="tokenToRefresh">The refresh token to be renewed.</param>
        /// <returns>The updated refresh token.</returns>
        /// <exception cref="ApplicationProcessException">Thrown when the <paramref name="tokenToRefresh"/> is null.</exception>
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
