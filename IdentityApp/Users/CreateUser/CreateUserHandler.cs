using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Endpoints.Responses;
using IdentityApp.Common.Exceptions;
using IdentityApp.Extensions;
using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Domain.Errors;
using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Users.CreateUser
{
    public record CreateUserRequest(string login, string password, string[] roles);
    public sealed class CreateUserHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ILogger<CreateUserHandler> logger) : ICreateUserHandler
    {
        public async Task<IResult> Handle(CreateUserRequest request)
        {
            if (await userRepository.IsLoginAlreadyExists(request.login))
                return UserErrors.UserAlreadyExistsError.ToProblemDetails();

            var user = await CreateUserWithRoles(request);

            await userRepository.CreateUserAsync(user);
            logger.LogInformation("User - {UserLogin} has been created.", user.Login);

            return ApiResponseFactory.Created("users",
                new CreateUserResponse(
                    request.login,
                    request.roles));
        }

        private async Task<User> CreateUserWithRoles(
            CreateUserRequest request)
        {
            var user = User.Create(request.login, request.password.Hash());

            var roles = await roleRepository.GetManyByNamesAsync(request.roles);

            foreach (var role in request.roles)
            {
                var roleState = roles.FirstOrDefault(x => x.Name == role);

                if (roleState is null)
                    throw new ApplicationProcessException("Cannot create user - invalid roles.");

                user.AddRole(roleState);
            }

            return user;
        }
    }
}
