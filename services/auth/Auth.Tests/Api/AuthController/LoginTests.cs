using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Tests for api/auth/login endpoint.
/// </summary>
public class LoginTests : BaseTests
{
    private readonly LoginRequest _request = new()
    {
        Username = "test",
        Password = "password"
    };

    private readonly AuthToken _token = new()
    {
        AccessToken = "access-token",
        RefreshToken = "refresh-token",
        ExpiresIn = 3600
    };

    [Fact]
    public async Task ReturnsOk_WhenLoginSucceeds()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LoginAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = true,
                Data = _token
            });

        // Act
        var result = await Controller.Login(_request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(_token);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LoginAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Invalid password",
                ErrorType = ErrorType.Validation
            });

        // Act
        var result = await Controller.Login(_request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LoginAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "User not found",
                ErrorType = ErrorType.NotFound
            });

        // Act
        var result = await Controller.Login(_request);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ReturnsForbidden_WhenUserIsBlocked()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LoginAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "User forbidden",
                ErrorType = ErrorType.Forbidden
            });

        // Act
        var result = await Controller.Login(_request);

        // Assert
        result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LoginAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Something went wrong",
                ErrorType = ErrorType.Internal
            });

        // Act
        var result = await Controller.Login(_request);

        // Assert
        var errorResult = result.Should().BeOfType<ObjectResult>().Subject;
        errorResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}