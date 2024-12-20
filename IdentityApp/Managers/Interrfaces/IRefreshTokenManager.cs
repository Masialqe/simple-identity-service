using IdentityApp.Users.Models;

namespace IdentityApp.Managers.Interrfaces
{
    public interface IRefreshTokenManager
    {
        Task<string> CreateRefreshToken(User user);
        string GenerateRefreshTokenValue();
        DateTime GenerateTokenExpirationDate();
    }
}