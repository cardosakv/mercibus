using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.UnitTests.Application.AuthService;

/// <summary>
/// Tests for auth service confirm email method.
/// </summary>
public class ConfirmEmailAsyncTests : BaseTests
{
    private readonly ConfirmEmailQuery _query = new()
    {
        UserId = "user-1",
        Token = "email-token"
    };

    private readonly User _user = new()
    {
        Id = "user-1",
        Email = "test@email.com",
        EmailConfirmed = false
    };

    [Fact]
    public async Task Success_WhenEmailConfirmed()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_query.UserId))
            .ReturnsAsync(_user);
        UserManagerMock
            .Setup(x => x.ConfirmEmailAsync(_user, It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await AuthService.ConfirmEmailAsync(_query);

        // Assert
        UserManagerMock.Verify(x => x.FindByIdAsync(_query.UserId), Times.Once);
        UserManagerMock.Verify(x => x.ConfirmEmailAsync(_user, It.IsAny<string>()), Times.Once);

        response.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_query.UserId))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.ConfirmEmailAsync(_query);

        // Assert
        UserManagerMock.Verify(x => x.FindByIdAsync(_query.UserId), Times.Once);
        UserManagerMock.Verify(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.UserNotFound);
    }

    [Fact]
    public async Task Fail_WhenConfirmationFails()
    {
        // Arrange
        var error = new IdentityError { Code = "InvalidToken" };

        UserManagerMock
            .Setup(x => x.FindByIdAsync(_query.UserId))
            .ReturnsAsync(_user);
        UserManagerMock
            .Setup(x => x.ConfirmEmailAsync(_user, It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(error));

        // Act
        var response = await AuthService.ConfirmEmailAsync(_query);

        // Assert
        UserManagerMock.Verify(x => x.FindByIdAsync(_query.UserId), Times.Once);
        UserManagerMock.Verify(x => x.ConfirmEmailAsync(_user, It.IsAny<string>()), Times.Once);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.TokenInvalid);
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_query.UserId))
            .ThrowsAsync(new Exception("An unexpected error occurred."));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            AuthService.ConfirmEmailAsync(_query));
    }
}