using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.Tests.Api.AuthController;

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
            .ReturnsAsync(new Response
            {
                IsSuccess = true,
                Message = Messages.UserLoggedOut
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
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Token revoked",
                ErrorType = ErrorType.Forbidden
            });

        // Act
        var result = await Controller.Logout(_request);

        // Assert
        result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LogoutAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Unexpected error",
                ErrorType = ErrorType.Internal
            });

        // Act
        var result = await Controller.Logout(_request);

        // Assert
        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}