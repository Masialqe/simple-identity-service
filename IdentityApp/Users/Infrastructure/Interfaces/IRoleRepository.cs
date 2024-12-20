using IdentityApp.Users.Models;

namespace IdentityApp.Users.Infrastructure.Interfaces
{
    public interface IRoleRepository
    {
        Task CreateRoleAsync(
            Role role, 
            CancellationToken cancellationToken = default);

        Task<bool> IsRoleAlreadyExists(
            string roleName);

        Task<IEnumerable<Role>> GetManyByNamesAsync(
            string[] names, 
            CancellationToken cancellationToken = default);
    }
}