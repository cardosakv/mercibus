using Auth.Application.DTOs;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.UnitTests.Api.AuthController;

/// <summary>
/// Tests for api/auth/logout endpoint.
/// </summary>
public class LogoutTests : BaseTests
{
    private readonly LogoutRequest _request = new()
    {
        RefreshToken = "valid-refresh-token"
    };

    [Fact]
    public async Task ReturnsOk_WhenLogoutSucceeds()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LogoutAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = true
            });

        // Act
        var result = await Controller.Logout(_request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ReturnsForbidden_WhenTokenIsInvalidOrRevoked()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LogoutAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.PermissionError,
                ErrorCode = ErrorCode.TokenInvalid
            });

        // Act
        var result = await Controller.Logout(_request);

        // Assert
        var response = result.Should().BeOfType<ObjectResult>().Subject;
        response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LogoutAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.ApiError,
                ErrorCode = ErrorCode.Internal
            });

        // Act
        var result = await Controller.Logout(_request);

        // Assert
        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}