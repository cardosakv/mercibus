using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Tests for auth service login method.
/// </summary>
public class LoginAsyncTests : BaseTests
{
    private readonly LoginRequest _request = new()
    {
        Username = "test",
        Password = "password"
    };

    private readonly AuthToken _authToken = new()
    {
        TokenType = "Bearer",
        AccessToken = "test-access-token",
        RefreshToken = "test-refresh-token",
        ExpiresIn = 600000
    };

    [Fact]
    public async Task Success_WhenUserLoggedIn()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User());
        UserManagerMock
            .Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        UserManagerMock
            .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync([Roles.Customer]);
        TokenServiceMock
            .Setup(x => x.CreateAccessToken(It.IsAny<User>(), It.IsAny<string>()))
            .Returns((_authToken.AccessToken, _authToken.ExpiresIn));
        RefreshTokenRepositoryMock
            .Setup(x => x.CreateTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(_authToken.RefreshToken);

        // Act
        var response = await AuthService.LoginAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.FindByNameAsync(_request.Username), Times.Once);
        UserManagerMock.Verify(x => x.CheckPasswordAsync(It.IsAny<User>(), _request.Password), Times.Once);
        UserManagerMock.Verify(x => x.GetRolesAsync(It.IsAny<User>()), Times.Once);
        TokenServiceMock.Verify(x => x.CreateAccessToken(It.IsAny<User>(), Roles.Customer), Times.Once);
        RefreshTokenRepositoryMock.Verify(x => x.CreateTokenAsync(It.IsAny<string>()), Times.Once);

        response.IsSuccess.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<AuthToken>();

        var token = response.Data.As<AuthToken>();
        token.AccessToken.Should().Be(_authToken.AccessToken);
        token.RefreshToken.Should().Be(_authToken.RefreshToken);
        token.ExpiresIn.Should().Be(_authToken.ExpiresIn);
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.LoginAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task Fail_WhenPasswordIsInvalid()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User());
        UserManagerMock
            .Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var response = await AuthService.LoginAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.CheckPasswordAsync(It.IsAny<User>(), _request.Password), Times.Once);
        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task Fail_WhenUserHasNoRoles()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User());
        UserManagerMock
            .Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        UserManagerMock
            .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>());

        // Act
        var response = await AuthService.LoginAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        const string exceptionMessage = "This is a test exception.";
        UserManagerMock
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var response = await AuthService.LoginAsync(_request);

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
        response.Data.Should().BeNull();
        response.ErrorType.Should().Be(ErrorType.Internal);
    }
}