using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Managers.Interrfaces;
using IdentityApp.Endpoints.Validation;
using IdentityApp.Endpoints.Responses;
using IdentityApp.Endpoints;
using FluentValidation;
using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Domain.Errors;
using IdentityApp.Shared.Domain.Models;


namespace IdentityApp.Users.LoginUser
{
    public record LoginUserRequest(string login, string password);
    public record LoginUserResponse(string token, string refreshToken);

    public static class LoginUser
    {
        public sealed class Validator : AbstractValidator<LoginUserRequest>
        {
            public Validator()
            {
                RuleFor(r => r.login).NotEmpty().MinimumLength(5);
                RuleFor(r => r.password).NotEmpty().MinimumLength(5);
            }
        }

        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("auth/login", LoginUserHandler)
                    .AddEndpointFilter<ValidationActionFilter<LoginUserRequest>>()
                    .WithTags("Authentication");
            }
        }

        public static async Task<IResult> LoginUserHandler(
            LoginUserRequest request, IUserRepository userRepository,
            IUserValidator userValidator,
            ITokenManager tokenManager, IHttpContextAccessor httpContextAccessor,
            ILogger<Endpoint> logger)
        {
            var user = await userRepository.GetUserByLoginAsync(
                request.login, httpContextAccessor.HttpContext?.RequestAborted ?? default);
            if (user is null)
                return UserErrors.UserDoesntExistsError.ToProblemDetails();

            var userValidationResult = ValidateUser(user, userValidator, request);
            if (userValidationResult.IsFailure)
                return await HandleValidationFailure(user, userRepository, userValidationResult);

            await UpdateUsersMetadata(user, userRepository, httpContextAccessor);

            var response = await CreateLoginUserResponse(user, tokenManager);

            logger.LogInformation("User {UserLogin} has been logged in.", user?.Login);

            return ApiResponseFactory.Ok(response);
        }

        private static async Task<LoginUserResponse> CreateLoginUserResponse(User? user, ITokenManager tokenManager)
        {
            (string accessToken, string refreshToken) = await tokenManager.GenerateAccessTokenWithRefreshToken(user);

            return new LoginUserResponse(accessToken, refreshToken);
        }

        private static async Task<IResult> HandleValidationFailure(
            User user,
            IUserRepository userRepository,
            Result userValidationResult)
        {
            await userRepository.UpdateUserAsync(user);
            return userValidationResult.ToProblemDetails();
        }

        private static async Task UpdateUsersMetadata(
            User? user,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            if (user is null)
                throw new ArgumentNullException("Cannot assign value to empty object.");

            user.SourceAddres = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()!;
            user.LoginAttemps = 0;

            await userRepository.UpdateUserAsync(user);
        }

        private static Result ValidateUser(
            User user,
            IUserValidator userValidator,
            LoginUserRequest request)
        {
            var validationResult = userValidator.Validate(user, request);
            return validationResult;
        }
    }
}
