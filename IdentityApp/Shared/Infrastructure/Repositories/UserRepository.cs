using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Infrastructure.Data;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace IdentityApp.Shared.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing <see cref="User"/> entities in the database.
    /// </summary>
    public class UserRepository(
        IdentityDbContext context) : IUserRepository
    {

        /// <summary>
        /// Creates a new <see cref="User"/> in the database.
        /// </summary>
        /// <param name="user">The user to be created.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <exception cref="ApplicationProcessException">Thrown when the user is null.</exception>
        public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            if (user is null) throw new ApplicationProcessException("User cannot be null.");

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a <see cref="User"/> by their login name.
        /// </summary>
        /// <param name="login">The login name of the user.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="User"/> if found, otherwise null.</returns>
        public async Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken = default)
        {
            return await context.Users
                        .Include(r => r.Roles)
                        .FirstOrDefaultAsync(x => x.Login == login, cancellationToken);
        }

        /// <summary>
        /// Updates an existing <see cref="User"/> in the database.
        /// </summary>
        /// <param name="user">The user to be updated.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <exception cref="ApplicationProcessException">Thrown when the user is null.</exception>
        public async Task UpdateUserAsync(User? user, CancellationToken cancellationToken = default)
        {
            if (user is null) throw new ApplicationProcessException("User cannot be null.");

            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Checks if a user with the specified login already exists in the database.
        /// </summary>
        /// <param name="userLogin">The login name of the user to check.</param>
        /// <returns>True if the login exists, otherwise false.</returns>
        public async Task<bool> IsLoginAlreadyExists(string userLogin)
            => await context.Users.AnyAsync(x => x.Login == userLogin);
    }

}
