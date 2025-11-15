using System.Text;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.UnitTests.Application.AuthService;

/// <summary>
/// Tests for auth service forgot password method.
/// </summary>
public class ForgotPasswordAsyncTests : BaseTests
{
    private readonly ForgotPasswordRequest _request = new()
    {
        Username = "test@email.com"
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
            .Setup(x => x.FindByNameAsync(_request.Username))
            .ReturnsAsync(_user);
        UserManagerMock
            .Setup(x => x.GeneratePasswordResetTokenAsync(_user))
            .ReturnsAsync(token);
        HttpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext());
        ConfigurationMock
            .Setup(x => x["Url:PasswordReset"])
            .Returns(resetUrl);
        EmailServiceMock
            .Setup(x => x.SendPasswordResetLink("test@gmail.com", It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        var response = await AuthService.ForgotPasswordAsync(_request);

        // Assert
        response.IsSuccess.Should().BeTrue();
        UserManagerMock.Verify(x => x.FindByNameAsync(_request.Username), Times.Once);
        UserManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(_user), Times.Once);
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByNameAsync(_request.Username))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.ForgotPasswordAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.UserNotFound);
    }

    [Fact]
    public async Task Fail_WhenHttpContextIsNull()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByNameAsync(_request.Username))
            .ReturnsAsync(_user);
        HttpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns((HttpContext?)null);

        // Act
        var response = await AuthService.ForgotPasswordAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.ApiError);
        response.ErrorCode.Should().Be(ErrorCode.Internal);

        UserManagerMock.Verify(x => x.FindByNameAsync(_request.Username), Times.Once);
        UserManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Never);
        EmailServiceMock.Verify(x => x.SendPasswordResetLink(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByNameAsync(_request.Username))
            .ThrowsAsync(new Exception("An unexpected error occurred."));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            AuthService.ForgotPasswordAsync(_request));
    }
}