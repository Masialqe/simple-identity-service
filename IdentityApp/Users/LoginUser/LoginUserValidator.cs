﻿using IdentityApp.Shared.Abstractions.ApiResults;
using IdentityApp.Shared.Domain.Errors;
using IdentityApp.Shared.Domain.Models;
using Microsoft.Extensions.Options;
using IdentityApp.Configuration;
using IdentityApp.Extensions;

namespace IdentityApp.Users.LoginUser
{
    /// <summary>
    /// Validates a user's credentials during the login process, ensuring the user is active and the password is correct.
    /// </summary>
    public sealed class LoginUserValidator(
        IOptions<UserVerificationOptions> options) : ILoginUserValidator
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


