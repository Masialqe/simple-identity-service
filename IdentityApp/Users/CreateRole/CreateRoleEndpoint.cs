using IdentityApp.Endpoints.Authentication;
using IdentityApp.Endpoints.Validation;
using IdentityApp.Endpoints;
using FluentValidation;

namespace IdentityApp.Users.CreateRole
{
    /// <summary>
    /// Validator for the <see cref="CreateRoleRequest"/> class.
    /// Ensures that the role name is valid.
    /// </summary>
    public sealed class Validator : AbstractValidator<CreateRoleRequest>
    {
        public Validator()
        {
            RuleFor(r => r.roleName).NotEmpty().MinimumLength(5).MaximumLength(48);
        }
    }

    /// <summary>
    /// Defines the endpoint for creating a new role.
    /// </summary>
    public sealed class CreateRoleEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("roles", async (CreateRoleRequest request, ICreateRoleHandler handler) =>
            {
                return await handler.Handle(request);
            })
                .AddEndpointFilter<ApiKeyEndpointFilter>()
                .AddEndpointFilter<ValidationActionFilter<CreateRoleRequest>>()
                .WithTags("Admin");
        }
    }
}
