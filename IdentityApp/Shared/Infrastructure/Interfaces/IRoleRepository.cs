using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Shared.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines methods for managing roles in the application.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Creates a new role asynchronously.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateRoleAsync(
            Role role,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a role with the specified name already exists.
        /// </summary>
        /// <param name="roleName">The name of the role to check for.</param>
        /// <returns><c>true</c> if the role already exists; otherwise, <c>false</c>.</returns>
        Task<bool> IsRoleAlreadyExists(
            string roleName);

        /// <summary>
        /// Retrieves a list of roles by their names.
        /// </summary>
        /// <param name="names">An array of role names to search for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of matching roles.</returns>
        Task<IEnumerable<Role>> GetManyByNamesAsync(
            string[] names,
            CancellationToken cancellationToken = default);
    }

}