using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Users.Models;

namespace IdentityApp.Users.Validators.UserValidators
{
    public interface IUserValidator
    {
        Result<User> Validate(User userToValidate, LoginUserRequest inputRequest);
    }
}