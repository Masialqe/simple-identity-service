using IdentityApp.Users.Infrastructure.Interfaces;
using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Endpoints.Authentication;
using IdentityApp.Endpoints.Validation;
using IdentityApp.Endpoints.Responses;
using IdentityApp.Users.Errors;
using IdentityApp.Users.Models;
using IdentityApp.Endpoints;
using FluentValidation;

namespace IdentityApp.Users
{
    public record CreateRoleRequest(string roleName);

    public static class CreateRole
    {

        public sealed class Validator : AbstractValidator<CreateRoleRequest>
        {
            public Validator()
            {
                RuleFor(r => r.roleName).NotEmpty().MinimumLength(5).MaximumLength(48);
            }
        }

        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("roles", Handler)
                    .AddEndpointFilter<ApiKeyEndpointFilter>()
                    .AddEndpointFilter<ValidationActionFilter<CreateRoleRequest>>()
                    .WithTags("Admin");
            }
        }
        public static async Task<IResult> Handler(
                 CreateRoleRequest request,
                 IRoleRepository roleRepository,
                 ILogger<Endpoint> logger)
        {
            var role = Role.Create(request.roleName);

            if (await roleRepository.IsRoleAlreadyExists(request.roleName))
                return Result.Failure(RoleErrors.RoleAlreadyExists).ToProblemDetails();

            await roleRepository.CreateRoleAsync(role);
            logger.LogInformation("Role - {RoleName} has been created.", role.Name);

            return ApiResponseFactory.Created("roles",
                new { name = role.Name });
        }
    }
}
