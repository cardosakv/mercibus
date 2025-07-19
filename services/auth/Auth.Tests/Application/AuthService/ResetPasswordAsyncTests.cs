using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

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
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.UserNotFound);
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
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.TokenInvalid);
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

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.ApiError);
    }
}