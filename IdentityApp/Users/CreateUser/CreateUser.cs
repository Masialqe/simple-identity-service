using IdentityApp.Endpoints.Authentication;
using IdentityApp.Endpoints.Validation;
using IdentityApp.Endpoints;
using FluentValidation;

namespace IdentityApp.Users.CreateUser
{
    public sealed class Validator : AbstractValidator<CreateUserRequest>
    {
        public Validator()
        {
            RuleFor(r => r.login).NotEmpty().MinimumLength(5).MaximumLength(48);
            RuleFor(r => r.password).NotEmpty().MinimumLength(5).MaximumLength(48);
            RuleFor(r => r.roles).NotEmpty();
        }
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users", async (CreateUserRequest request, ICreateUserHandler handler) =>
            {
                return await handler.Handle(request);
            })
                .AddEndpointFilter<ApiKeyEndpointFilter>()
                .AddEndpointFilter<ValidationActionFilter<CreateUserRequest>>()
                .WithTags("Admin");
        }
    }
}

