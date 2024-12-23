using IdentityApp.Users.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Common.Exceptions;
using IdentityApp.Users.Models;
using IdentityApp.Common.Data;

namespace IdentityApp.Users.Infrastructure.Repositories
{
    public class RoleRepository(
        IdentityDbContext context) : IRoleRepository
    {
        public async Task CreateRoleAsync(
            Role role, CancellationToken cancellationToken = default)
        {
            if (role == null) throw new ApplicationProcessException();

            await context.Roles.AddAsync(role, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> IsRoleAlreadyExists(string roleName)
            => await context.Roles.AnyAsync(x => x.Name == roleName);

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
