using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Abstractions.ApiResults;
using IdentityApp.Shared.Managers.Interrfaces;
using IdentityApp.Shared.Domain.Errors;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Endpoints.Responses;

namespace IdentityApp.Users.LoginUser
{
    public record LoginUserRequest(string login, string password);
    public class LoginUserHandler(
        IUserRepository userRepository,
        ILoginUserValidator userValidator,
        ITokenManager tokenManager, IHttpContextAccessor httpContextAccessor,
        ILogger<LoginUserHandler> logger) : ILoginUserHandler
    {
        public async Task<IResult> Handle(LoginUserRequest request)
        {
            var user = await userRepository.GetUserByLoginAsync(
                request.login, httpContextAccessor.HttpContext?.RequestAborted ?? default);
            if (user is null)
                return UserErrors.UserDoesntExistsError.ToProblemDetails();

            var userValidationResult = ValidateUser(user, request);
            if (userValidationResult.IsFailure)
                return await HandleValidationFailure(user, userValidationResult);

            await UpdateUsersMetadata(user);

            var response = await CreateLoginUserResponse(user);

            logger.LogInformation("User {UserLogin} has been logged in.", user?.Login);

            return ApiResponseFactory.Ok(response);
        }

        private async Task<LoginUserResponse> CreateLoginUserResponse(User? user)
        {
            (string accessToken, string refreshToken) = await tokenManager.GenerateAccessTokenWithRefreshToken(user);

            return new LoginUserResponse(accessToken, refreshToken);
        }

        private async Task<IResult> HandleValidationFailure(
            User user,
            Result userValidationResult)
        {
            await userRepository.UpdateUserAsync(user);
            return userValidationResult.ToProblemDetails();
        }

        private async Task UpdateUsersMetadata(User? user)
        {
            if (user is null)
                throw new ArgumentNullException("Cannot assign value to empty object.");

            user.SourceAddres = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()!;
            user.LoginAttemps = 0;

            await userRepository.UpdateUserAsync(user);
        }

        private Result ValidateUser(
            User user,
            LoginUserRequest request)
        {
            var validationResult = userValidator.Validate(user, request);
            return validationResult;
        }
    }
}
