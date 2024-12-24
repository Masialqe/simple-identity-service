using IdentityApp.Shared.Abstractions.ApiResults;
using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Users.RefreshUser
{
    public interface IRefreshUserValidator
    {
        Task<Result> ValidateAsync(
            RefreshToken? refreshToken,
            RefreshUserRequest request);
    }
}