using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Users.LoginUser
{
    public interface IUserValidator
    {
        Result<User> Validate(User? userToValidate, LoginUserRequest inputRequest);
    }
}