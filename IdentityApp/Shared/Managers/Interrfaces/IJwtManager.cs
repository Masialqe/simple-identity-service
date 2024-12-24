using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Shared.Managers.Interrfaces
{
    public interface IJwtManager
    {
        string CreateAccessToken(User user);
    }
}