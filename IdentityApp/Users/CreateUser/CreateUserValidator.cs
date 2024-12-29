using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Abstractions.ApiResults;
using IdentityApp.Shared.Domain.Errors;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using IdentityApp.Configuration;

namespace IdentityApp.Users.CreateUser
{
    /// <summary>
    /// Validates the user creation request, ensuring that the login is unique and the password meets the required criteria.
    /// </summary>
    public sealed class CreateUserValidator(
        IUserRepository userRepository,
        IOptions<PasswordOptions> options) : ICreateUserValidator
    {
        public async Task<Result> ValidateAsync(CreateUserRequest request)
        {
            if (await userRepository.IsLoginAlreadyExists(request.login))
                return UserErrors.UserAlreadyExistsError;

            var passwordOptions = options.Value;

            if (!IsPasswordMatchingRequirements(request.password, passwordOptions))
                return UserErrors.UsersPasswordDoesntMatchRequirementsError(passwordOptions.PasswordErrorMessage!);

            return Result.Success();
        }

        public bool IsPasswordMatchingRequirements(string password, PasswordOptions options)
        {
            if (password.Length < options.PasswordLength) return false;
            if (!Regex.IsMatch(password, options.PasswordRegex!)) return false;

            return true;
        }
    }
}
