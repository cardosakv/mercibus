using System.Text;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Tests for auth service forgot password method.
/// </summary>
public class ForgotPasswordAsyncTests : BaseTests
{
    private readonly ForgotPasswordRequest _request = new()
    {
        Email = "test@email.com"
    };

    private readonly User _user = new()
    {
        Id = "user-1",
        Email = "test@email.com"
    };

    [Fact]
    public async Task Success_WhenResetLinkSent()
    {
        // Arrange
        var token = "reset-token";
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var resetUrl = "https://redirect.com/reset";

        UserManagerMock
            .Setup(x => x.FindByEmailAsync(_request.Email))
            .ReturnsAsync(_user);

        UserManagerMock
            .Setup(x => x.GeneratePasswordResetTokenAsync(_user))
            .ReturnsAsync(token);

        HttpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext());

        ConfigurationMock
            .Setup(x => x["RedirectUrl:PasswordReset"])
            .Returns(resetUrl);

        EmailServiceMock
            .Setup(x => x.SendPasswordResetLink(_request.Email, It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        var response = await AuthService.ForgotPasswordAsync(_request);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be(Messages.PasswordResetLinkSent);

        UserManagerMock.Verify(x => x.FindByEmailAsync(_request.Email), Times.Once);
        UserManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(_user), Times.Once);
        EmailServiceMock.Verify(x => x.SendPasswordResetLink(_request.Email, It.Is<string>(link => link.Contains(resetUrl))), Times.Once);
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByEmailAsync(_request.Email))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.ForgotPasswordAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.NotFound);
        response.Message.Should().Be(Messages.UserNotFound);

        UserManagerMock.Verify(x => x.FindByEmailAsync(_request.Email), Times.Once);
        UserManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Never);
        EmailServiceMock.Verify(x => x.SendPasswordResetLink(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Fail_WhenHttpContextIsNull()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByEmailAsync(_request.Email))
            .ReturnsAsync(_user);

        HttpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns((HttpContext?)null);

        // Act
        var response = await AuthService.ForgotPasswordAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Internal);
        response.Message.Should().Be(Messages.UnexpectedError);

        UserManagerMock.Verify(x => x.FindByEmailAsync(_request.Email), Times.Once);
        UserManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Never);
        EmailServiceMock.Verify(x => x.SendPasswordResetLink(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Fail_WhenSendingEmailFails()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByEmailAsync(_request.Email))
            .ReturnsAsync(_user);

        UserManagerMock
            .Setup(x => x.GeneratePasswordResetTokenAsync(_user))
            .ReturnsAsync("token");

        HttpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext());

        ConfigurationMock
            .Setup(x => x["RedirectUrl:PasswordReset"])
            .Returns("https://redirect.com/reset");

        EmailServiceMock
            .Setup(x => x.SendPasswordResetLink(_request.Email, It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var response = await AuthService.ForgotPasswordAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Internal);
        response.Message.Should().Be(Messages.UnexpectedError);

        UserManagerMock.Verify(x => x.FindByEmailAsync(_request.Email), Times.Once);
        UserManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(_user), Times.Once);
        EmailServiceMock.Verify(x => x.SendPasswordResetLink(_request.Email, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByEmailAsync(_request.Email))
            .ThrowsAsync(new Exception("unexpected"));

        // Act
        var response = await AuthService.ForgotPasswordAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Internal);
        response.Message.Should().Be(Messages.UnexpectedError);

        UserManagerMock.Verify(x => x.FindByEmailAsync(_request.Email), Times.Once);
        LoggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("unexpected")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
}