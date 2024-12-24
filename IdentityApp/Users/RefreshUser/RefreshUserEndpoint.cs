using IdentityApp.Endpoints.Validation;
using IdentityApp.Endpoints;
using FluentValidation;

namespace IdentityApp.Users.RefreshUser
{
    public sealed class Validator : AbstractValidator<RefreshUserRequest>
    {
        public Validator()
        {
            RuleFor(r => r.refreshToken).NotEmpty();
        }
    }

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