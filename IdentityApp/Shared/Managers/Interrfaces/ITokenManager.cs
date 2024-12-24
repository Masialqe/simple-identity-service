using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Shared.Managers.Interrfaces
{
    public interface ITokenManager
    {
        string GenerateAccessToken(User? user);
        Task<(string, string)> GenerateAccessTokenWithRefreshToken(User? user);
        Task<RefreshToken> RenewRefreshTokenAsync(RefreshToken? tokenToRefresh);
    }
}