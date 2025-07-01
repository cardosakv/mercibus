using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Tests for auth service reset password method.
/// </summary>
public class ResetPasswordAsyncTests : BaseTests
{
    private readonly ResetPasswordRequest _request = new()
    {
        UserId = "user-1",
        Token = "token",
        NewPassword = "new-password"
    };

    private readonly User _user = new()
    {
        Id = "user-1",
        Email = "test@email.com"
    };

    [Fact]
    public async Task Success_WhenPasswordReset()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_request.UserId))
            .ReturnsAsync(_user);

        UserManagerMock
            .Setup(x => x.ResetPasswordAsync(_user, _request.Token, _request.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await AuthService.ResetPasswordAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.FindByIdAsync(_request.UserId), Times.Once);
        UserManagerMock.Verify(x => x.ResetPasswordAsync(_user, _request.Token, _request.NewPassword), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Once);

        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be(Messages.PasswordResetSuccess);
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_request.UserId))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.ResetPasswordAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.FindByIdAsync(_request.UserId), Times.Once);
        UserManagerMock.Verify(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Never);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.NotFound);
        response.Message.Should().Be(Messages.UserNotFound);
    }

    [Fact]
    public async Task Fail_WhenResetFails()
    {
        // Arrange
        var error = new IdentityError { Code = "InvalidToken", Description = "Token is invalid" };

        UserManagerMock
            .Setup(x => x.FindByIdAsync(_request.UserId))
            .ReturnsAsync(_user);

        UserManagerMock
            .Setup(x => x.ResetPasswordAsync(_user, _request.Token, _request.NewPassword))
            .ReturnsAsync(IdentityResult.Failed(error));

        // Act
        var response = await AuthService.ResetPasswordAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.FindByIdAsync(_request.UserId), Times.Once);
        UserManagerMock.Verify(x => x.ResetPasswordAsync(_user, _request.Token, _request.NewPassword), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Never);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(IdentityErrorMapper.MapToErrorType(error.Code));
        response.Message.Should().Be(error.Description);
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_request.UserId))
            .ThrowsAsync(new Exception("unexpected error"));

        // Act
        var response = await AuthService.ResetPasswordAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        TransactionServiceMock.Verify(x => x.RollbackAsync(), Times.Once);
        LoggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Internal);
        response.Message.Should().Be(Messages.UnexpectedError);
    }
}