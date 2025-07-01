using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Tests for auth service send confirmation email method.
/// </summary>
public class SendConfirmationEmailAsyncTests : BaseTests
{
    private readonly SendConfirmationEmailRequest _request = new()
    {
        Email = "test@email.com"
    };

    private readonly User _user = new()
    {
        Id = "user-1",
        Email = "test@email.com",
        EmailConfirmed = false
    };

    [Fact]
    public async Task Success_WhenEmailIsSent()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByEmailAsync(_request.Email))
            .ReturnsAsync(_user);

        UserManagerMock
            .Setup(x => x.GenerateEmailConfirmationTokenAsync(_user))
            .ReturnsAsync("sample-token");

        HttpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext());

        EmailServiceMock
            .Setup(x => x.SendEmailConfirmationLink(_request.Email, It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        var response = await AuthService.SendConfirmationEmailAsync(_request);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be(Messages.EmailConfirmationSent);
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByEmailAsync(_request.Email))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.SendConfirmationEmailAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.NotFound);
        response.Message.Should().Be(Messages.UserNotFound);
    }

    [Fact]
    public async Task Fail_WhenEmailAlreadyConfirmed()
    {
        // Arrange
        var confirmedUser = _user;
        confirmedUser.EmailConfirmed = true;

        UserManagerMock
            .Setup(x => x.FindByEmailAsync(_request.Email))
            .ReturnsAsync(confirmedUser);

        // Act
        var response = await AuthService.SendConfirmationEmailAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Conflict);
        response.Message.Should().Be(Messages.EmailAlreadyVerified);
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
        var response = await AuthService.SendConfirmationEmailAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Internal);
    }

    [Fact]
    public async Task Fail_WhenSendingEmailFails()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByEmailAsync(_request.Email))
            .ReturnsAsync(_user);

        UserManagerMock
            .Setup(x => x.GenerateEmailConfirmationTokenAsync(_user))
            .ReturnsAsync("sample-token");

        HttpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext());

        EmailServiceMock
            .Setup(x => x.SendEmailConfirmationLink(_request.Email, It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var response = await AuthService.SendConfirmationEmailAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Internal);
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByEmailAsync(_request.Email))
            .ThrowsAsync(new Exception("unexpected"));

        // Act
        var response = await AuthService.SendConfirmationEmailAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Internal);
        response.Message.Should().Be(Messages.UnexpectedError);
    }
}