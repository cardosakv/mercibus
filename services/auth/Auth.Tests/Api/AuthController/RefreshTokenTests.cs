using Auth.Application.DTOs;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Tests for api/auth/refresh-token endpoint.
/// </summary>
public class RefreshTokenTests : BaseTests
{
    private readonly RefreshRequest _request = new()
    {
        RefreshToken = "valid-refresh-token"
    };

    private readonly AuthTokenResponse _token = new()
    {
        AccessToken = "new-access-token",
        RefreshToken = "new-refresh-token",
        ExpiresIn = 3600
    };

    [Fact]
    public async Task ReturnsOk_WhenTokenIsRefreshed()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.RefreshTokenAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = true,
                Data = _token
            });

        // Act
        var result = await Controller.RefreshToken(_request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(_token);
    }

    [Fact]
    public async Task ReturnsForbidden_WhenTokenIsRevoked()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.RefreshTokenAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.PermissionError,
                ErrorCode = ErrorCode.TokenInvalid
            });

        // Act
        var result = await Controller.RefreshToken(_request);

        // Assert
        result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task ReturnsUnauthorized_WhenTokenIsExpired()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.RefreshTokenAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.AuthenticationError,
                ErrorCode = ErrorCode.RefreshTokenExpired
            });

        // Act
        var result = await Controller.RefreshToken(_request);

        // Assert
        var unauthorized = result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
        unauthorized.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenSomethingFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.RefreshTokenAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.ApiError,
                ErrorCode = ErrorCode.Internal
            });

        // Act
        var result = await Controller.RefreshToken(_request);

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}