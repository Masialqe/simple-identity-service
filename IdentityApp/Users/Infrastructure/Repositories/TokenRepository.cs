using IdentityApp.Users.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Common.Exceptions;
using IdentityApp.Users.Models;
using IdentityApp.Common.Data;

namespace IdentityApp.Users.Infrastructure.Repositories
{
    public class TokenRepository(
        IdentityDbContext context) : ITokenRepository
    {
        public async Task CreateRefreshTokenAsync(
            RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            if (refreshToken is null) throw new ApplicationProcessException();

            await context.AddAsync(refreshToken, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken?> GetRefreshTokenByValueAsync(
            string tokenValue, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tokenValue)) throw new ApplicationProcessException();

            var result = await context.RefreshTokens
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(x => x.Token == tokenValue);

            return result;
        }

        public async Task<string?> GetNewestRefreshTokenPerUserAsync(
            Guid userId, CancellationToken cancellationToken = default)
        {
            var result = await context.RefreshTokens
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(t => t.ExpiresOnUtc)
                    .Select(x => x.Token)
                    .FirstAsync();

            return result;
        }

        public async Task UpdateRefreshTokenAsync(
            RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            if (refreshToken is null) throw new ApplicationProcessException();

            context.RefreshTokens.Attach(refreshToken);
            context.Entry(refreshToken).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRefreshTokensPerUserAsync(
            Guid userId, CancellationToken cancellationToken = default)
        {
            await context.RefreshTokens
                .Where(x => x.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
