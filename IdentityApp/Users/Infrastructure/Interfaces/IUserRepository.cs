using IdentityApp.Users.Models;

namespace IdentityApp.Users.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(User? user, CancellationToken cancellationToken = default);
        Task<bool> IsLoginAlreadyExists(string userLogin);
    }
}