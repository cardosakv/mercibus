using Auth.Application.DTOs;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.UnitTests.Application.AuthService;

/// <summary>
/// Tests for auth service refresh token method.
/// </summary>
public class RefreshTokenAsyncTests : BaseTests
{
    private readonly RefreshRequest _request = new()
    {
        RefreshToken = "valid-refresh-token"
    };

    [Fact]
    public async Task Success_WhenTokenValidAndUserFound()
    {
        // Arrange
        var token = new RefreshToken
        {
            TokenHash = _request.RefreshToken,
            IsRevoked = false,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            UserId = "user-1"
        };

        var user = new User { Id = "user-1" };
        var roles = new List<string> { Roles.Customer };

        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(_request.RefreshToken))
            .ReturnsAsync(token);

        UserManagerMock
            .Setup(x => x.FindByIdAsync(token.UserId))
            .ReturnsAsync(user);

        UserManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        TokenServiceMock
            .Setup(x => x.CreateAccessToken(user, roles.First()))
            .Returns(("access-token", 3600));

        RefreshTokenRepositoryMock
            .Setup(x => x.RotateTokenAsync(token))
            .ReturnsAsync("new-refresh-token");

        // Act
        var response = await AuthService.RefreshTokenAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Once);
        response.IsSuccess.Should().BeTrue();

        var tokenData = response.Data as AuthTokenResponse;
        tokenData.Should().NotBeNull();
        tokenData!.AccessToken.Should().Be("access-token");
        tokenData.RefreshToken.Should().Be("new-refresh-token");
    }

    [Fact]
    public async Task Fail_WhenTokenNotFoundOrRevoked()
    {
        // Arrange
        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(_request.RefreshToken))
            .ReturnsAsync((RefreshToken?)null);

        // Act
        var response = await AuthService.RefreshTokenAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.TokenInvalid);
    }

    [Fact]
    public async Task Fail_WhenTokenIsExpired()
    {
        // Arrange
        var expiredToken = new RefreshToken
        {
            TokenHash = _request.RefreshToken,
            IsRevoked = false,
            ExpiresAt = DateTime.UtcNow.AddMinutes(-1),
            UserId = "user-1"
        };

        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(_request.RefreshToken))
            .ReturnsAsync(expiredToken);

        RefreshTokenRepositoryMock
            .Setup(x => x.RevokeTokenAsync(expiredToken))
            .ReturnsAsync(true);

        // Act
        var response = await AuthService.RefreshTokenAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.AuthenticationError);
        response.ErrorCode.Should().Be(ErrorCode.RefreshTokenExpired);
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        var token = new RefreshToken
        {
            TokenHash = _request.RefreshToken,
            IsRevoked = false,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            UserId = "user-1"
        };

        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(_request.RefreshToken))
            .ReturnsAsync(token);

        UserManagerMock
            .Setup(x => x.FindByIdAsync(token.UserId))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.RefreshTokenAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.UserNotFound);
    }

    [Fact]
    public async Task Fail_WhenUserHasNoRole()
    {
        // Arrange
        var token = new RefreshToken
        {
            TokenHash = _request.RefreshToken,
            IsRevoked = false,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            UserId = "user-1"
        };

        var user = new User { Id = "user-1" };

        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(_request.RefreshToken))
            .ReturnsAsync(token);

        UserManagerMock
            .Setup(x => x.FindByIdAsync(token.UserId))
            .ReturnsAsync(user);

        UserManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string>());

        // Act
        var response = await AuthService.RefreshTokenAsync(_request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.PermissionError);
        response.ErrorCode.Should().Be(ErrorCode.UserNoRoleAssigned);
    }

    [Fact]
    public async Task Fail_WhenExceptionThrown()
    {
        // Arrange
        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("An unexpected error occurred."));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            AuthService.RefreshTokenAsync(_request));
    }
}