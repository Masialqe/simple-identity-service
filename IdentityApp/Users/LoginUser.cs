using IdentityApp.Users.Infrastructure.Interfaces;
using IdentityApp.Users.Validators.UserValidators;
using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Managers.Interrfaces;
using IdentityApp.Endpoints.Validation;
using IdentityApp.Endpoints.Responses;
using IdentityApp.Users.Models;
using IdentityApp.Endpoints;
using FluentValidation;


namespace IdentityApp.Users
{
    public record LoginUserRequest(string login, string password);
    public record LoginUserResponse(string token, string refreshToken);

    public static class LoginUser
    {
        public sealed class Validator: AbstractValidator<LoginUserRequest>
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

            var userValidationResult = userValidator.Validate(user, request);

            if (userValidationResult.IsFailure)
            {
                await userRepository.UpdateUserAsync(user);
                return userValidationResult.ToProblemDetails();
            }

            UpdateUsersMetadata(user, httpContextAccessor);
            await userRepository.UpdateUserAsync(user);

            var response = await CreateLoginUserResponse(user, tokenManager);
            logger.LogInformation("User {UserLogin} has been logged in.", user?.Login);

            return ApiResponseFactory.Ok(response);
        }

        private static async Task<LoginUserResponse> CreateLoginUserResponse(User? user, ITokenManager tokenManager)
        {
            (string accessToken, string refreshToken) = await tokenManager.GenerateAccessTokenWithRefreshToken(user);

            return new LoginUserResponse(accessToken, refreshToken);
        }

        private static void UpdateUsersMetadata(User? user, IHttpContextAccessor httpContextAccessor)
        {
            if(user is null)
                throw new ArgumentNullException("Cannot assign value to empty object.");

            user.SourceAddres = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()!;
            user.LoginAttemps = 0;
        }
    }
}
