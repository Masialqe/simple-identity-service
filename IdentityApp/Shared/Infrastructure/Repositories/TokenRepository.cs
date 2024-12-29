using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Infrastructure.Data;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Shared.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing <see cref="RefreshToken"/> entities in the database.
    /// </summary>
    public class TokenRepository(
        IdentityDbContext context) : ITokenRepository
    {

        /// <summary>
        /// Creates a new <see cref="RefreshToken"/> in the database.
        /// </summary>
        /// <param name="refreshToken">The refresh token to be created.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <exception cref="ApplicationProcessException">Thrown when the refresh token is null.</exception>
        public async Task CreateRefreshTokenAsync(
            RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            if (refreshToken is null) throw new ApplicationProcessException("Refresh token cannot be null.");

            await context.AddAsync(refreshToken, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a <see cref="RefreshToken"/> by its value.
        /// </summary>
        /// <param name="tokenValue">The value of the refresh token.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="RefreshToken"/> if found, otherwise null.</returns>
        /// <exception cref="ApplicationProcessException">Thrown when the token value is invalid or empty.</exception>
        public async Task<RefreshToken?> GetRefreshTokenByValueAsync(
            string tokenValue, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tokenValue)) throw new ApplicationProcessException("Token value cannot be null or empty.");

            var result = await context.RefreshTokens
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(x => x.Token == tokenValue, cancellationToken);

            return result;
        }

        /// <summary>
        /// Retrieves the newest refresh token associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose token is being retrieved.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>The newest refresh token, or null if none found.</returns>
        public async Task<string?> GetNewestRefreshTokenPerUserAsync(
            Guid userId, CancellationToken cancellationToken = default)
        {
            var result = await context.RefreshTokens
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(t => t.ExpiresOnUtc)
                    .Select(x => x.Token)
                    .FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        /// <summary>
        /// Updates an existing <see cref="RefreshToken"/> in the database.
        /// </summary>
        /// <param name="refreshToken">The refresh token to be updated.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <exception cref="ApplicationProcessException">Thrown when the refresh token is null.</exception>
        public async Task UpdateRefreshTokenAsync(
            RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            if (refreshToken is null) throw new ApplicationProcessException("Refresh token cannot be null.");

            context.RefreshTokens.Attach(refreshToken);
            context.Entry(refreshToken).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes all refresh tokens associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose tokens are being deleted.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        public async Task DeleteRefreshTokensPerUserAsync(
            Guid userId, CancellationToken cancellationToken = default)
        {
            await context.RefreshTokens
                .Where(x => x.UserId == userId)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }

}
