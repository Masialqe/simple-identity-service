using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Users.RefreshUser
{
    public interface IRefreshTokenValidator
    {
        Task<Result> ValidateAsync(
            RefreshToken? refreshToken,
            RefreshUserRequest request);
    }
}