using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Shared.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines methods for managing user entities in the application.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateUserAsync(
            User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a user by their login.
        /// </summary>
        /// <param name="login">The login of the user to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the user if found, or <c>null</c> if no user matches the login.</returns>
        Task<User?> GetUserByLoginAsync(
            string login, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing user in the repository asynchronously.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateUserAsync(
            User? user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a user login already exists.
        /// </summary>
        /// <param name="userLogin">The login of the user to check.</param>
        /// <returns><c>true</c> if a user with the provided login already exists; otherwise, <c>false</c>.</returns>
        Task<bool> IsLoginAlreadyExists(string userLogin);
    }

}