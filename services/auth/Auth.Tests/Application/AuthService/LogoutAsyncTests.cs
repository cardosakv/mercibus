using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Tests for auth service logout method.
/// </summary>
public class LogOutAsyncTests : BaseTests
{
    private readonly LogoutRequest _request = new()
    {
        RefreshToken = "sample-refresh-token"
    };

    [Fact]
    public async Task Success_WhenTokenRevoked()
    {
        // Arrange
        var token = new RefreshToken
        {
            IsRevoked = false,
            TokenHash = "test-token-hash",
            UserId = "test-user-id"
        };

        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(_request.RefreshToken))
            .ReturnsAsync(token);

        RefreshTokenRepositoryMock
            .Setup(x => x.RevokeTokenAsync(token))
            .ReturnsAsync(true);

        // Act
        var response = await AuthService.LogoutAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        RefreshTokenRepositoryMock.Verify(x => x.RevokeTokenAsync(token), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Once);
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be(Messages.UserLoggedOut);
    }

    [Fact]
    public async Task Fail_WhenTokenIsNullOrRevoked()
    {
        // Arrange
        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(_request.RefreshToken))
            .ReturnsAsync((RefreshToken?)null);

        // Act
        var response = await AuthService.LogoutAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        RefreshTokenRepositoryMock.Verify(x => x.RevokeTokenAsync(It.IsAny<RefreshToken>()), Times.Never);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Never);
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Forbidden);
    }

    [Fact]
    public async Task Fail_WhenTokenRevocationFails()
    {
        // Arrange
        var token = new RefreshToken
        {
            IsRevoked = false,
            TokenHash = "test-token-hash",
            UserId = "test-user-id"
        };

        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(_request.RefreshToken))
            .ReturnsAsync(token);

        RefreshTokenRepositoryMock
            .Setup(x => x.RevokeTokenAsync(token))
            .ReturnsAsync(false);

        // Act
        var response = await AuthService.LogoutAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        RefreshTokenRepositoryMock.Verify(x => x.RevokeTokenAsync(token), Times.Once);
        TransactionServiceMock.Verify(x => x.RollbackAsync(), Times.Once);
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Internal);
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        const string error = "Unexpected exception occurs.";
        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception(error));

        // Act
        var response = await AuthService.LogoutAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        TransactionServiceMock.Verify(x => x.RollbackAsync(), Times.Once);
        LoggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(error)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
        response.ErrorType.Should().Be(ErrorType.Internal);
    }
}