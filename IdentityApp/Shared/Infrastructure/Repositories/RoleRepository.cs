using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Infrastructure.Data;
using IdentityApp.Shared.Domain.Models;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Shared.Exceptions;

namespace IdentityApp.Shared.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing <see cref="Role"/> entities in the database.
    /// </summary>
    public class RoleRepository(
        IdentityDbContext context) : IRoleRepository
    {
        /// <summary>
        /// Creates a new <see cref="Role"/> in the database.
        /// </summary>
        /// <param name="role">The role to be created.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <exception cref="ApplicationProcessException">Thrown when the role is null.</exception>
        public async Task CreateRoleAsync(Role role, CancellationToken cancellationToken = default)
        {
            if (role == null) throw new ApplicationProcessException("Role cannot be null.");

            await context.Roles.AddAsync(role, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Checks if a role with the specified name already exists in the database.
        /// </summary>
        /// <param name="roleName">The name of the role to check.</param>
        /// <returns>True if the role exists; otherwise, false.</returns>
        public async Task<bool> IsRoleAlreadyExists(string roleName)
            => await context.Roles.AnyAsync(x => x.Name == roleName);

        /// <summary>
        /// Retrieves multiple roles by their names.
        /// </summary>
        /// <param name="names">The names of the roles to retrieve.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A collection of roles that match the specified names.</returns>
        public async Task<IEnumerable<Role>> GetManyByNamesAsync(
            string[] names,
            CancellationToken cancellationToken = default)
        {
            var result = await context.Roles
                .Where(x => names.Contains(x.Name))
                .ToListAsync(cancellationToken);

            return result;
        }
    }

}
