using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Users.Models;

namespace IdentityApp.Users.Validators.RefreshTokenValidators
{
    public interface IRefreshTokenValidator
    {
        Task<Result> ValidateAsync(RefreshToken? refreshToken, RefreshUserRequest request);
    }
}