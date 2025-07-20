using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.UnitTests.Application.AuthService;

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
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.TokenInvalid);
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
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.ApiError);
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        RefreshTokenRepositoryMock
            .Setup(x => x.RetrieveTokenAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("An unexpected error occurred."));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            AuthService.LogoutAsync(_request));
    }
}