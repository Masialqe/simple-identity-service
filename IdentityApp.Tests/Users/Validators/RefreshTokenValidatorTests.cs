using IdentityApp.Shared.Infrastructure.Interfaces;
using IdentityApp.Shared.Abstractions.Errors;
using IdentityApp.Shared.Domain.Errors;
using IdentityApp.Shared.Domain.Models;
using IdentityApp.Users.RefreshUser;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using NSubstitute;
using System.Net;

namespace IdentityApp.Tests.Users.Validators
{
    public class RefreshTokenValidatorTests
    {
        private readonly ITokenRepository tokenRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RefreshUserValidator refreshTokenValidator;
        private readonly RefreshUserRequest refreshTokenRequest;

        private readonly string ValidToken = "validToken";
        private readonly string ValidIpAddress = "127.0.0.1";

        public RefreshTokenValidatorTests()
        {
            tokenRepository = Substitute.For<ITokenRepository>();
            httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            refreshTokenValidator = new RefreshUserValidator(tokenRepository, httpContextAccessor);

            refreshTokenRequest = new RefreshUserRequest(ValidToken);
        }

        [Fact]
        public async Task ValidateRefreshToken_ShouldReturnError_WhenRefreshTokenIsNull()
        {
            //Arrange
            RefreshToken? token = null;

            //Act
            var validationResult = await refreshTokenValidator.ValidateAsync(token, refreshTokenRequest);

            //Assert
            validationResult.Should().NotBeNull();
            validationResult.IsFailure.Should().BeTrue();
            validationResult.Error.Should().NotBeNull();
            validationResult.Error.Should().Be(TokenErrors.InvalidRefreshTokenError);
        }

        [Fact]
        public async Task ValidateRefreshToken_ShouldReturnError_WhenRefreshTokenExpired()
        {
            //Arrange
            var refreshToken = GetRefreshToken(User.Create("", ""), -1);

            //Act
            var validationResult = await refreshTokenValidator.ValidateAsync(refreshToken, refreshTokenRequest);

            //Assert
            validationResult.Should().NotBeNull();
            validationResult.IsFailure.Should().BeTrue();
            validationResult.Error.Should().NotBeNull();
            validationResult.Error.Should().Be(TokenErrors.InvalidRefreshTokenError);
        }

        [Fact]
        public async Task ValidateRefreshToken_ShouldReturnError_WhenTokenIsNotNewestOne()
        {
            //Arrange
            var user = GetExampleUser(ValidIpAddress);

            var refreshToken = GetRefreshToken(user);

            tokenRepository.GetNewestRefreshTokenPerUserAsync(refreshToken.UserId).Returns("differentToken");

            SetupHttpContext(ValidIpAddress);

            //Act
            var validationResult = await refreshTokenValidator.ValidateAsync(refreshToken, refreshTokenRequest);

            //Assert
            validationResult.Should().NotBeNull();
            validationResult.IsFailure.Should().BeTrue();
            validationResult.Error.Should().NotBeNull();
            validationResult.Error.Should().Be(TokenErrors.AccessDeniedError);
        }

        [Fact]
        public async Task ValidateRefreshToken_ShouldReturnError_WhenUserIpChanged()
        {
            //Arrange
            var user = GetExampleUser(ValidIpAddress);

            var refreshToken = GetRefreshToken(user);

            tokenRepository.GetNewestRefreshTokenPerUserAsync(refreshToken.UserId).Returns(ValidToken);

            SetupHttpContext("192.168.0.1");

            //Act
            var validationResult = await refreshTokenValidator.ValidateAsync(refreshToken, refreshTokenRequest);

            //Assert
            validationResult.Should().NotBeNull();
            validationResult.IsFailure.Should().BeTrue();
            validationResult.Error.Should().NotBeNull();
            validationResult.Error.Should().Be(TokenErrors.AccessDeniedError);
        }

        [Fact]
        public async Task ValidateRefreshToken_ShouldDeleteAllTokens_WhenReauthenticationNeeded()
        {
            //Arrange
            var user = GetExampleUser(ValidIpAddress);

            var refreshToken = GetRefreshToken(user);

            tokenRepository.GetNewestRefreshTokenPerUserAsync(refreshToken.UserId).Returns("differentToken");

            SetupHttpContext(ValidIpAddress);

            //Act
            var validationResult = await refreshTokenValidator.ValidateAsync(refreshToken, refreshTokenRequest);

            //Assert
            await tokenRepository.Received(1).DeleteRefreshTokensPerUserAsync(refreshToken.UserId);
        }

        [Fact]
        public async Task ValidateRefreshToken_ShouldNotReturnError_WhenAllConditionsMet()
        {
            //Arrange
            var user = GetExampleUser(ValidIpAddress);

            var refreshToken = GetRefreshToken(user);

            tokenRepository.GetNewestRefreshTokenPerUserAsync(refreshToken.UserId).Returns(ValidToken);

            SetupHttpContext(ValidIpAddress);

            //Act
            var validationResult = await refreshTokenValidator.ValidateAsync(refreshToken, refreshTokenRequest);

            //Assert
            validationResult.Should().NotBeNull();
            validationResult.IsSuccess.Should().BeTrue();
            validationResult.Error.Should().Be(Error.None);
        }

        private void SetupHttpContext(string ipAddress)
        {
            var context = new DefaultHttpContext();
            context.Connection.RemoteIpAddress = IPAddress.Parse(ipAddress);
            httpContextAccessor.HttpContext.Returns(context);
        }
        private RefreshToken GetRefreshToken(User user, int expireIn = 1)
        {
            var refreshToken = RefreshToken.Create(ValidToken, user, DateTime.UtcNow.AddMinutes(expireIn));
            refreshToken.User = user;

            return refreshToken;
        }
        private User GetExampleUser(string ipAddr)
        {
            var user = User.Create("", "");
            user.SourceAddres = ipAddr;

            return user;
        }
    }
}
