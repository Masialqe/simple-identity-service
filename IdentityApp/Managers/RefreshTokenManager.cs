using IdentityApp.Users.Infrastructure.Interfaces;
using IdentityApp.Managers.Interrfaces;
using System.Security.Cryptography;
using IdentityApp.Users.Models;

namespace IdentityApp.Managers
{
    public sealed class RefreshTokenManager(
        ITokenRepository tokenRepository,
        IConfiguration configuration) : IRefreshTokenManager
    {
        private const int REFRESH_TOKEN_LENGTH = 128;

        public async Task<string> CreateRefreshToken(User user)
        {
            var refreshTokenValue = GenerateRefreshTokenValue();

            var tokenExpireOnUtc = GenerateTokenExpirationDate();
            var refreshTokenEntity = RefreshToken.Create(refreshTokenValue, user, tokenExpireOnUtc);

            await tokenRepository.CreateRefreshTokenAsync(refreshTokenEntity);

            return refreshTokenValue;
        }

        public string GenerateRefreshTokenValue()
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(REFRESH_TOKEN_LENGTH));

        public DateTime GenerateTokenExpirationDate()
        {
            var tokenExpireOnUtc = configuration.GetValue<int>("Jwt:RefreshTokenExpireTimeInDays");
            return DateTime.UtcNow.AddDays(tokenExpireOnUtc);
        }
    }
}
