using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Endpoints.Validation;
using IdentityApp.Managers.Interrfaces;
using IdentityApp.Endpoints.Responses;
using IdentityApp.Endpoints;
using FluentValidation;
using IdentityApp.Shared.Infrastructure.Interfaces;

namespace IdentityApp.Users.RefreshUser
{
    public record RefreshUserRequest(string refreshToken);
    public record RefreshUserResponse(string token, string refreshToken);

    public static class RefreshUser
    {
        public sealed class Endpoint : IEndpoint
        {
            public sealed class Validator : AbstractValidator<RefreshUserRequest>
            {
                public Validator()
                {
                    RuleFor(r => r.refreshToken).NotEmpty();
                }
            }

            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("auth/refresh-token", Handler)
                    .AddEndpointFilter<ValidationActionFilter<RefreshUserRequest>>()
                    .WithTags("Authentication");
            }

            public static async Task<IResult> Handler(
                RefreshUserRequest request,
                ITokenRepository tokenRepository,
                IRefreshTokenValidator refreshTokenValidator,
                IHttpContextAccessor httpContextAccessor,
                ITokenManager tokenManager, ILogger<Endpoint> logger)
            {
                var refreshTokenState = await tokenRepository.GetRefreshTokenByValueAsync(
                    request.refreshToken, httpContextAccessor.HttpContext?.RequestAborted ?? default);

                var refreshTokenValidationResult = await refreshTokenValidator.ValidateAsync(refreshTokenState, request);

                if (refreshTokenValidationResult.IsFailure)
                    return refreshTokenValidationResult.ToProblemDetails();

                var accessToken = tokenManager.GenerateAccessToken(refreshTokenState?.User);
                var renevedRefreshToken = await tokenManager.RenewRefreshTokenAsync(refreshTokenState);

                var response = new RefreshUserResponse(accessToken, renevedRefreshToken.Token);
                logger.LogInformation("User {UserLogin} has been logged in.", refreshTokenState?.User?.Login);

                return ApiResponseFactory.Ok(response);
            }
        }
    }
}