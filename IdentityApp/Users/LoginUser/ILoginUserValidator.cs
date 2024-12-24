using IdentityApp.Shared.Abstractions.ApiResults;
using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Users.LoginUser
{
    public interface ILoginUserValidator
    {
        Result<User> Validate(User? userToValidate, LoginUserRequest inputRequest);
    }
}