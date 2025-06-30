using System.Text;
using Auth.Application.Common;
using Auth.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Moq;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Tests for auth service confirm email method.
/// </summary>
public class ConfirmEmailAsyncTests : BaseTests
{
    private readonly string _userId = "user-1";
    private readonly string _rawToken = "token-123";
    private readonly string _encodedToken;

    private readonly User _user = new()
    {
        Id = "user-1",
        Email = "test@email.com",
        EmailConfirmed = false
    };

    public ConfirmEmailAsyncTests()
    {
        _encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(_rawToken));
    }

    [Fact]
    public async Task Success_WhenEmailConfirmed()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ReturnsAsync(_user);

        UserManagerMock
            .Setup(x => x.ConfirmEmailAsync(_user, _rawToken))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await AuthService.ConfirmEmailAsync(_userId, _encodedToken);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be(Messages.EmailVerified);
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.ConfirmEmailAsync(_userId, _encodedToken);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.NotFound);
        response.Message.Should().Be(Messages.UserNotFound);
    }

    [Fact]
    public async Task Fail_WhenConfirmationFails()
    {
        // Arrange
        var error = new IdentityError { Code = "InvalidToken", Description = "Invalid token" };

        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ReturnsAsync(_user);

        UserManagerMock
            .Setup(x => x.ConfirmEmailAsync(_user, _rawToken))
            .ReturnsAsync(IdentityResult.Failed(error));

        // Act
        var response = await AuthService.ConfirmEmailAsync(_userId, _encodedToken);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Message.Should().Be(error.Description);
        response.ErrorType.Should().Be(IdentityErrorMapper.MapToErrorType(error.Code));
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ThrowsAsync(new Exception("Unexpected"));

        // Act
        var response = await AuthService.ConfirmEmailAsync(_userId, _encodedToken);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Internal);
        response.Message.Should().Be(Messages.UnexpectedError);
    }
}