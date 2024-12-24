using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Managers.Interrfaces;
using IdentityApp.Shared.Domain.Models;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using IdentityApp.Configuration;

namespace IdentityApp.Shared.Managers
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
