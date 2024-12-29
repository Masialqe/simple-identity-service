using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Abstractions.ApiResults;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Endpoints.Responses;
using IdentityApp.Shared.Exceptions;
using IdentityApp.Extensions;

namespace IdentityApp.Users.CreateUser
{
    public record CreateUserRequest(string login, string password, string[] roles);
    public sealed class CreateUserHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ICreateUserValidator createUserValidator,
        ILogger<CreateUserHandler> logger) : ICreateUserHandler
    {
        public async Task<IResult> Handle(CreateUserRequest request)
        {
            var createUserValidationResult = await createUserValidator.ValidateAsync(request);

            if (createUserValidationResult.IsFailure)
                return createUserValidationResult.ToProblemDetails();

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
