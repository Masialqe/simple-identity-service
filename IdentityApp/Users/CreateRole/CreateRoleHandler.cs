using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Abstractions.ApiResults;
using IdentityApp.Shared.Domain.Errors;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Endpoints.Responses;

namespace IdentityApp.Users.CreateRole
{
    public record CreateRoleRequest(string roleName);

    public class CreateRoleHandler(
        IRoleRepository roleRepository,
        ILogger<CreateRoleHandler> logger) : ICreateRoleHandler
    {
        public async Task<IResult> Handle(CreateRoleRequest request)
        {
            var role = Role.Create(request.roleName);

            if (await roleRepository.IsRoleAlreadyExists(request.roleName))
                return RoleErrors.RoleAlreadyExists.ToProblemDetails();

            await roleRepository.CreateRoleAsync(role);
            logger.LogInformation("Role - {RoleName} has been created.", role.Name);

            return ApiResponseFactory.Created("roles",
                new CreateRoleResponse(role.Name));
        }
    }
}
