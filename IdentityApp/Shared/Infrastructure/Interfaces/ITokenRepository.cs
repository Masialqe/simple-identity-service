using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Shared.Infrastructure.Interfaces
{
    public interface ITokenRepository
    {
        Task CreateRefreshTokenAsync(
            RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task<string?> GetNewestRefreshTokenPerUserAsync(
            Guid userId, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetRefreshTokenByValueAsync(
            string tokenValue, CancellationToken cancellationToken = default);
        Task UpdateRefreshTokenAsync(
            RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task DeleteRefreshTokensPerUserAsync(
            Guid userId, CancellationToken cancellationToken = default);
    }
}