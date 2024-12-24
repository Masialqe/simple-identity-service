using IdentityApp.Shared.Abstractions.Errors;
using IdentityApp.Shared.Domain.Errors;
using IdentityApp.Shared.Domain.Models;
using Microsoft.Extensions.Options;
using IdentityApp.Users.LoginUser;
using IdentityApp.Configuration;
using IdentityApp.Extensions;
using FluentAssertions;
using NSubstitute;

namespace IdentityApp.Tests.Users.Validators
{
    public class UserValidatorTests
    {
        private readonly LoginUserRequest loginRequest;
        private readonly LoginUserValidator userValidator;

        public UserValidatorTests()
        {
            var options = Substitute.For<IOptions<UserVerificationOptions>>();
            options.Value.Returns(new UserVerificationOptions
            {
                MaxLoginAttempts = 3,
                BlockUserExpirationTimeInMinutes = 1
            });

            userValidator = new LoginUserValidator(options);
            loginRequest = new LoginUserRequest("test", "test123");
        }

        [Fact]
        public void ValidateUser_ShouldReturnError_WhenUserIsNull()
        {
            //Arrange
            User? user = null;

            //Act
            var validationResult = userValidator.Validate(user, loginRequest);

            //Assert
            validationResult.Should().NotBeNull();
            validationResult.IsFailure.Should().BeTrue();
            validationResult.Error.Should().NotBeNull();
            validationResult.Error.Should().Be(UserErrors.InvalidUserError);
        }

        [Fact]
        public void ValidateUser_ShouldReturnError_WhenUserIsBlocked()
        {
            //Arrange
            var user = User.Create("", "");
            user.IsActive = false;
            user.BlockExpireOnUtc = DateTime.UtcNow.AddMinutes(2);

            //Act
            var validationResult = userValidator.Validate(user, loginRequest);

            //Assert
            validationResult.Should().NotBeNull();
            validationResult.IsFailure.Should().BeTrue();
            validationResult.Error.Should().NotBeNull();
            validationResult.Error.Should().Be(UserErrors.UserBlockedError);
        }

        [Fact]
        public void ValidateUser_ShouldReturnError_WhenPasswordIsInvalid()
        {
            //Arrange
            var correctPassword = "correctPassword";
            var correctPasswordHash = correctPassword.Hash();

            var user = User.Create("correctLogin", correctPasswordHash);

            //Act
            var validationResult = userValidator.Validate(user, loginRequest);

            //Assert
            validationResult.Should().NotBeNull();
            validationResult.IsFailure.Should().BeTrue();
            validationResult.Error.Should().NotBeNull();
            validationResult.Error.Should().Be(UserErrors.InvalidUserError);
        }

        [Fact]
        public void ValidateUser_ShouldNotReturnError_WhenUserIsCorrectAndHasBeenBlockedAndBlockExpires()
        {
            //Arrange
            var correctPassword = "test123".Hash();
            var correctUser = User.Create("correctUser", correctPassword);
            correctUser.IsActive = false;
            correctUser.BlockExpireOnUtc = DateTime.UtcNow.AddMinutes(-2);

            //Act
            var validationResult = userValidator.Validate(correctUser, loginRequest);

            //Assert
            validationResult.Should().NotBeNull();
            validationResult.IsSuccess.Should().BeTrue();
            validationResult.Error.Should().Be(Error.None);
        }

        [Fact]
        public void ValidateUser_ShouldBlockUser_WhenTooManyLoginAttempts()
        {
            //Arrange
            var wrongPassword = "test1234321".Hash();
            var correctUser = User.Create("correctUser", wrongPassword);
            correctUser.IsActive = true;
            correctUser.LoginAttemps = 3;

            //Act
            var validationResult = userValidator.Validate(correctUser, loginRequest);

            //Assert
            validationResult.Should().NotBeNull();
            validationResult.IsFailure.Should().BeTrue();
            correctUser.IsActive.Should().BeFalse();
        }

        [Fact]
        public void ValidateUser_ShouldUnlockUser_WhenIsBlockedAndBlockExpires()
        {
            //Arrange
            var correctPassword = "test123".Hash();
            var correctUser = User.Create("correctUser", correctPassword);
            correctUser.IsActive = false;
            correctUser.BlockExpireOnUtc = DateTime.UtcNow.AddMinutes(-2);

            //Act
            var validationResult = userValidator.Validate(correctUser, loginRequest);

            //Assert
            validationResult.Should().NotBeNull();
            correctUser.IsActive.Should().BeTrue();
        }

        [Fact]
        public void ValidateUser_ShouldResetLoginAttempts_WhenUnlockindUser()
        {
            //Arrange
            var correctPassword = "test123".Hash();
            var correctUser = User.Create("correctUser", correctPassword);
            correctUser.IsActive = false;
            correctUser.LoginAttemps = 3;
            correctUser.BlockExpireOnUtc = DateTime.UtcNow.AddMinutes(-2);

            //Act
            var validationResult = userValidator.Validate(correctUser, loginRequest);

            //Assert
            validationResult.Should().NotBeNull();
            correctUser.LoginAttemps.Should().Be(0);
        }

        [Fact]
        public void ValidateUser_ShouldIncrementLoginAttempts_WhenValidationFailed()
        {
            //Arrange
            var correctPassword = "correctPassword";
            var correctPasswordHash = correctPassword.Hash();

            var user = User.Create("correctLogin", correctPasswordHash);
            user.LoginAttemps = 0;

            //Act
            var validationResult = userValidator.Validate(user, loginRequest);

            //Assert
            validationResult.Should().NotBeNull();
            user.LoginAttemps.Should().Be(1);
            
        }
    }
}
