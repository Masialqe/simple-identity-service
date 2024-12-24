using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Shared.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(
            User user, CancellationToken cancellationToken = default);
        Task<User?> GetUserByLoginAsync(
            string login, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(
            User? user, CancellationToken cancellationToken = default);
        Task<bool> IsLoginAlreadyExists(string userLogin);
    }
}