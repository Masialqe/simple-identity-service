using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Managers.Interrfaces;
using IdentityApp.Shared.Domain.Models;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using IdentityApp.Configuration;

namespace IdentityApp.Shared.Managers
{
    /// <summary>
    /// A service for managing refresh tokens, including creating and generating refresh tokens.
    /// </summary>
    public sealed class RefreshTokenManager(
        ITokenRepository tokenRepository,
        IOptions<JwtOptions> options) : IRefreshTokenManager
    {
        private const int RefreshTokenLength = 128;

        /// <summary>
        /// Creates a new refresh token for the specified <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user for whom the refresh token is being created.</param>
        /// <returns>A string representing the generated refresh token.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="user"/> is null.</exception>
        public async Task<string> CreateRefreshToken(User user)
        {
            var refreshTokenValue = GenerateRefreshTokenValue();

            var tokenExpireOnUtc = GenerateTokenExpirationDate();
            var refreshTokenEntity = RefreshToken.Create(refreshTokenValue, user, tokenExpireOnUtc);

            await tokenRepository.CreateRefreshTokenAsync(refreshTokenEntity);

            return refreshTokenValue;
        }
        /// <summary>
        /// Generates a random refresh token value.
        /// </summary>
        /// <returns>A string representing the generated refresh token value.</returns>
        public string GenerateRefreshTokenValue()
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(RefreshTokenLength));

        /// <summary>
        /// Generates the expiration date for the refresh token.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the UTC expiration date of the refresh token.</returns>
        public DateTime GenerateTokenExpirationDate()
        {
            var refreshTokenExpireTimeInDays = options.Value.RefreshTokenExpireTimeInDays;
            return DateTime.UtcNow.AddDays(refreshTokenExpireTimeInDays);
        }
    }
}
