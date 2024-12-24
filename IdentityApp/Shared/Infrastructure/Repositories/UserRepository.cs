using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Infrastructure.Data;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace IdentityApp.Shared.Infrastructure.Repositories
{
    public class UserRepository(
        IdentityDbContext context) : IUserRepository
    {

        public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            if (user is null) throw new ApplicationProcessException();

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken = default)
        {
            return await context.Users
                        .Include(r => r.Roles)
                        .FirstOrDefaultAsync(x => x.Login == login, cancellationToken);
        }

        public async Task UpdateUserAsync(User? user, CancellationToken cancellationToken = default)
        {
            if (user is null) throw new ApplicationProcessException();

            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);
        }
        public async Task<bool> IsLoginAlreadyExists(string userLogin)
            => await context.Users.AnyAsync(x => x.Login == userLogin);
    }
}
