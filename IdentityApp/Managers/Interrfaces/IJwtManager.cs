using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Managers.Interrfaces
{
    public interface IJwtManager
    {
        string CreateAccessToken(User user);
    }
}