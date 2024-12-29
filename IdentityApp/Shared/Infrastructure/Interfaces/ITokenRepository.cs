using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Shared.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines methods for managing refresh tokens in the application.
    /// </summary>
    public interface ITokenRepository
    {
        /// <summary>
        /// Creates a new refresh token asynchronously.
        /// </summary>
        /// <param name="refreshToken">The refresh token to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateRefreshTokenAsync(
            RefreshToken refreshToken, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the newest refresh token for a given user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose latest refresh token is to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the newest refresh token or <c>null</c> if no token is found.</returns>
        Task<string?> GetNewestRefreshTokenPerUserAsync(
            Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a refresh token by its value.
        /// </summary>
        /// <param name="tokenValue">The value of the refresh token to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the matching refresh token or <c>null</c> if not found.</returns>
        Task<RefreshToken?> GetRefreshTokenByValueAsync(
            string tokenValue, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing refresh token in the repository.
        /// </summary>
        /// <param name="refreshToken">The refresh token to update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateRefreshTokenAsync(
            RefreshToken refreshToken, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes all refresh tokens associated with a given user.
        /// </summary>
        /// <param name="userId">The ID of the user whose refresh tokens are to be deleted.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteRefreshTokensPerUserAsync(
            Guid userId, CancellationToken cancellationToken = default);
    }

}