using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Users.CreateRole;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using NSubstitute;

namespace IdentityApp.Tests.Users.Handlers
{
    public class CreateRoleHandlerTests
    {
        private readonly IRoleRepository roleRepository;
        private readonly ILogger<CreateRoleHandler> logger;
        private readonly CreateRoleHandler createRoleHandler;

        public CreateRoleHandlerTests()
        {
            roleRepository = Substitute.For<IRoleRepository>();
            logger = Substitute.For<ILogger<CreateRoleHandler>>();

            createRoleHandler = new CreateRoleHandler(roleRepository, logger);
        }

        [Fact]
        public async Task CreateRole_ShouldReturnConflict_WhenRoleAlreadyExists()
        {
            // Arrange
            var request = new CreateRoleRequest("AdminRole");
            roleRepository.IsRoleAlreadyExists(request.roleName).Returns(true);

            // Act
            var result = await createRoleHandler.Handle(request);

            // Assert
            result.Should().NotBeNull();
            var statusCode = GetStatusCode(result);
            statusCode.Should().Be(409);

            await roleRepository.DidNotReceive().CreateRoleAsync(Arg.Any<Role>());
        }

        [Fact]
        public async Task CreateRole_ShouldCreateRoleSuccessfully_WhenInputIsValid()
        {
            // Arrange
            var request = new CreateRoleRequest("NewRole");
            roleRepository.IsRoleAlreadyExists(request.roleName).Returns(false);

            // Act
            var result = await createRoleHandler.Handle(request);

            // Assert
            result.Should().NotBeNull();
            var statusCode = GetStatusCode(result);
            statusCode.Should().Be(201);

            await roleRepository.Received(1).CreateRoleAsync(Arg.Is<Role>(r => r.Name == request.roleName));
        }

        private static int GetStatusCode(IResult result)
        {
            return result switch
            {
                IStatusCodeHttpResult statusCodeResult => statusCodeResult.StatusCode ?? 0,
                _ => 0
            };
        }
    }
}
