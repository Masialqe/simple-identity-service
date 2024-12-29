using IdentityApp.Endpoints.Validation;
using IdentityApp.Endpoints;
using FluentValidation;

namespace IdentityApp.Users.RefreshUser
{
    /// <summary>
    /// Validator for the <see cref="RefreshUserRequest"/> class.
    /// Ensures that the refresh token is not empty.
    /// </summary>
    public sealed class Validator : AbstractValidator<RefreshUserRequest>
    {
        public Validator()
        {
            RuleFor(r => r.refreshToken).NotEmpty();
        }
    }

    /// <summary>
    /// Defines the endpoint for user login authentication using refresh token.
    /// </summary>
    public sealed class RefreshUserEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/refresh-token", async (RefreshUserRequest request, IRefreshUserHandler handler) =>
            {
                return await handler.Handle(request);
            })
               .AddEndpointFilter<ValidationActionFilter<RefreshUserRequest>>()
               .WithTags("Authentication");
        }
    }
}