using Auth.Application.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RegisterRequest = Auth.Application.DTOs.RegisterRequest;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Tests for api/auth/register endpoint.
/// </summary>
public class RegisterTests : BaseTests
{
    private readonly RegisterRequest _request = new()
    {
        Username = "test",
        Password = "password",
        Email = "test@email.com"
    };

    [Fact]
    public async Task ReturnsOk_WhenRegistrationSucceeds()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.RegisterAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = true,
                Message = "User registered successfully."
            });

        // Act
        var result = await Controller.Register(_request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ReturnsConflict_WhenUserAlreadyExists()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.RegisterAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Username already taken.",
                ErrorType = ErrorType.Conflict
            });

        // Act
        var result = await Controller.Register(_request);

        // Assert
        var conflictResult = result.Should().BeOfType<ConflictObjectResult>().Subject;
        conflictResult.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.RegisterAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Password is too short.",
                ErrorType = ErrorType.Validation
            });

        // Act
        var result = await Controller.Register(_request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.RegisterAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Unexpected failure.",
                ErrorType = ErrorType.Internal
            });

        // Act
        var result = await Controller.Register(_request);

        // Assert
        var errorResult = result.Should().BeOfType<ObjectResult>().Subject;
        errorResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}