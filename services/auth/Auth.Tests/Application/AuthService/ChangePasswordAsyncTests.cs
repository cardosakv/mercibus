using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Tests for auth service change password method.
/// </summary>
public class ChangePasswordAsyncTests : BaseTests
{
    private readonly ChangePasswordRequest _request = new()
    {
        UserId = "user-1",
        CurrentPassword = "current-password",
        NewPassword = "new-password"
    };

    private readonly User _user = new()
    {
        Id = "user-1",
        Email = "test@email.com"
    };

    [Fact]
    public async Task Success_WhenPasswordChanged()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_request.UserId))
            .ReturnsAsync(_user);

        UserManagerMock
            .Setup(x => x.ChangePasswordAsync(_user, _request.CurrentPassword, _request.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await AuthService.ChangePasswordAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.FindByIdAsync(_request.UserId), Times.Once);
        UserManagerMock.Verify(x => x.ChangePasswordAsync(_user, _request.CurrentPassword, _request.NewPassword), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Once);

        response.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_request.UserId))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.ChangePasswordAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.FindByIdAsync(_request.UserId), Times.Once);
        UserManagerMock.Verify(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Never);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.UserNotFound);
    }

    [Fact]
    public async Task Fail_WhenPasswordChangeFails()
    {
        // Arrange
        var error = new IdentityError { Code = "InvalidPassword", Description = "Password doesn't meet criteria." };

        UserManagerMock
            .Setup(x => x.FindByIdAsync(_request.UserId))
            .ReturnsAsync(_user);

        UserManagerMock
            .Setup(x => x.ChangePasswordAsync(_user, _request.CurrentPassword, _request.NewPassword))
            .ReturnsAsync(IdentityResult.Failed(error));

        // Act
        var response = await AuthService.ChangePasswordAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.FindByIdAsync(_request.UserId), Times.Once);
        UserManagerMock.Verify(x => x.ChangePasswordAsync(_user, _request.CurrentPassword, _request.NewPassword), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Never);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.PasswordInvalid);
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_request.UserId))
            .ThrowsAsync(new Exception("unexpected exception"));

        // Act
        var response = await AuthService.ChangePasswordAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Never);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.ApiError);
        response.ErrorCode.Should().Be(ErrorCode.Internal);
    }
}