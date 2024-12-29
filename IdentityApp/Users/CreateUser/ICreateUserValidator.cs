using IdentityApp.Configuration;
using IdentityApp.Shared.Abstractions.ApiResults;

namespace IdentityApp.Users.CreateUser
{
    public interface ICreateUserValidator
    {
        bool IsPasswordMatchingRequirements(string password, PasswordOptions options);
        Task<Result> ValidateAsync(CreateUserRequest request);
    }
}