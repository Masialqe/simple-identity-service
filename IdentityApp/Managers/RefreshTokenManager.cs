using IdentityApp.Users.Infrastructure.Interfaces;
using IdentityApp.Managers.Interrfaces;
using IdentityApp.Common.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using IdentityApp.Users.Models;

namespace IdentityApp.Managers
{
    public sealed class RefreshTokenManager(
        ITokenRepository tokenRepository,
        IOptions<JwtOptions> options) : IRefreshTokenManager
    {
        private const int RefreshTokenLength = 128;

        public async Task<string> CreateRefreshToken(User user)
        {
            var refreshTokenValue = GenerateRefreshTokenValue();

            var tokenExpireOnUtc = GenerateTokenExpirationDate();
            var refreshTokenEntity = RefreshToken.Create(refreshTokenValue, user, tokenExpireOnUtc);

            await tokenRepository.CreateRefreshTokenAsync(refreshTokenEntity);

            return refreshTokenValue;
        }

        public string GenerateRefreshTokenValue()
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(RefreshTokenLength));

        public DateTime GenerateTokenExpirationDate()
        {
            var refreshTokenExpireTimeInDays = options.Value.RefreshTokenExpireTimeInDays;
            return DateTime.UtcNow.AddDays(refreshTokenExpireTimeInDays);
        }
    }
}
