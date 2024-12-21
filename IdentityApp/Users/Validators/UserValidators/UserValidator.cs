using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Common.Configuration;
using Microsoft.Extensions.Options;
using IdentityApp.Users.Errors;
using IdentityApp.Users.Models;
using IdentityApp.Extensions;

namespace IdentityApp.Users.Validators.UserValidators
{
    public sealed class UserValidator(
        IOptions<UserVerificationOptions> options) : IUserValidator
    {
        public Result<User> Validate(User? userToValidate, LoginUserRequest inputRequest)
        {
            if (userToValidate == null)
                return UserErrors.InvalidUserError;

            var statusResult = HandleBlockedUser(userToValidate);
            if (statusResult.IsFailure) return statusResult.Error;

            var passwordResult = ValidatePassword(userToValidate, inputRequest.password);
            if (passwordResult.IsFailure) return passwordResult.Error;

            return userToValidate;
        }

        private Result<User> HandleBlockedUser(User user)
        {
            if (!user.IsActive)
            {
                if (user.BlockExpireOnUtc > DateTime.UtcNow)
                    return UserErrors.UserBlockedError;

                user.LoginAttemps = 0;
                user.IsActive = true;
            }

            return user;
        }

        private Result<User> ValidatePassword(User user, string inputPassword)
        {
            if (!inputPassword.IsHashEqualTo(user.PasswordHash))
            {
                user.LoginAttemps++;
                UpdateLoginAttempts(user);
                return UserErrors.InvalidUserError;
            }

            return user;
        }

        private void UpdateLoginAttempts(User user)
        {
            var maxLogginAttempts = options.Value.MaxLoginAttempts;
            var blockExpireOnUtc = options.Value.BlockUserExpirationTimeInMinutes;

            if (user.LoginAttemps >= maxLogginAttempts)
            {
                user.IsActive = false;
                user.BlockExpireOnUtc = DateTime.UtcNow.AddMinutes(blockExpireOnUtc);
            }
        }
    }
}


