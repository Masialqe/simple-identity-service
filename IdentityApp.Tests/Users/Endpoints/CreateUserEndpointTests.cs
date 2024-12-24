using IdentityApp.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using NSubstitute;
using IdentityApp.Users.CreateUser;
using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Domain.Models;

namespace IdentityApp.Tests.Users.Endpoints
{
    public class CreateUserEndpointTests
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly ILogger<CreateUser.Endpoint> logger;

        public CreateUserEndpointTests()
        {
            userRepository = Substitute.For<IUserRepository>();
            roleRepository = Substitute.For<IRoleRepository>();
            logger = Substitute.For<ILogger<CreateUser.Endpoint>>();
        }

        [Fact]
        public async Task CreateUser_ShouldReturnConflict_WhenUserAlreadyExists()
        {
            // Arrange
            var request = new CreateUserRequest("existingUser", "password123", new[] { "Admin" });
            userRepository.IsLoginAlreadyExists(request.login).Returns(true);

            // Act
            var result = await CreateUser.Handler(request, userRepository, roleRepository, logger);

            // Assert
            result.Should().NotBeNull();
            var statusCode = GetStatusCode(result);
            statusCode.Should().Be(409);

            await userRepository.DidNotReceive().CreateUserAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreated_WhenUserCreatedSuccessfully()
        {
            // Arrange
            var request = new CreateUserRequest("newUser", "password123", new[] { "Admin" });
            userRepository.IsLoginAlreadyExists(request.login).Returns(false);
            roleRepository.GetManyByNamesAsync(request.roles).Returns(new[]
            {
                new Role { Name = "Admin" }
            });

            // Act
            var result = await CreateUser.Handler(request, userRepository, roleRepository, logger);

            // Assert
            result.Should().NotBeNull();
            var statusCode = GetStatusCode(result);
            statusCode.Should().Be(201);

            await userRepository.Received(1).CreateUserAsync(Arg.Is<User>(u => u.Login == request.login));
        }

        [Fact]
        public async Task CreateUser_ShouldThrowException_WhenInvalidRoleProvided()
        {
            // Arrange
            var request = new CreateUserRequest("newUser", "password123", new[] { "InvalidRole" });
            userRepository.IsLoginAlreadyExists(request.login).Returns(false);
            roleRepository.GetManyByNamesAsync(request.roles).Returns(Array.Empty<Role>());

            // Act
            Func<Task> action = async () => await CreateUser.Handler(request, userRepository, roleRepository, logger);

            // Assert
            await action.Should().ThrowAsync<ApplicationProcessException>()
                .WithMessage("Cannot create user - invalid roles.");

            await userRepository.DidNotReceive().CreateUserAsync(Arg.Any<User>());
        }

        private static int GetStatusCode(IResult result)
        {
            if (result is IStatusCodeHttpResult statusCodeResult)
            {
                return statusCodeResult.StatusCode ?? 0;
            }
            return 0;
        }
    }
}
