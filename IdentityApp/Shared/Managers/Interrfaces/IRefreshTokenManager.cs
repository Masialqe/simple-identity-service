using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Shared.Managers.Interrfaces
{
    public interface IRefreshTokenManager
    {
        Task<string> CreateRefreshToken(User user);
        string GenerateRefreshTokenValue();
        DateTime GenerateTokenExpirationDate();
    }
}