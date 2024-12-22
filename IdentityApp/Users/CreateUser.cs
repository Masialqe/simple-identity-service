using IdentityApp.Users.Infrastructure.Interfaces;
using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Endpoints.Authentication;
using IdentityApp.Endpoints.Validation;
using IdentityApp.Endpoints.Responses;
using IdentityApp.Common.Exceptions;
using IdentityApp.Users.Errors;
using IdentityApp.Users.Models;
using IdentityApp.Extensions;
using IdentityApp.Endpoints;
using FluentValidation;

namespace IdentityApp.Users
{
    public record CreateUserRequest(string login, string password, string[] roles);
    public record CreateUserResponse(string login, string[] roles);

    public static class CreateUser
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
                app.MapPost("users", Handler)
                    .AddEndpointFilter<ApiKeyEndpointFilter>()
                    .AddEndpointFilter<ValidationActionFilter<CreateUserRequest>>()
                    .WithTags("Admin");
            }
        }
        public static async Task<IResult> Handler(
                  CreateUserRequest request,
                  IUserRepository userRepository,
                  IRoleRepository roleRepository,
                  ILogger<Endpoint> logger)
        {
            if (await userRepository.IsLoginAlreadyExists(request.login))
                return Result.Failure(UserErrors.UserAlreadyExistsError).ToProblemDetails();

            var user = await CreateUserWithRoles(request, roleRepository);

            await userRepository.CreateUserAsync(user);
            logger.LogInformation("User - {UserLogin} has been created.", user.Login);

            return ApiResponseFactory.Created("users", 
                new CreateUserResponse(
                    request.login,
                    request.roles));
        }

        private static async Task<User> CreateUserWithRoles(
            CreateUserRequest request, IRoleRepository roleRepository)
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

