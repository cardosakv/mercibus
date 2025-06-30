using System.Security.Claims;
using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Tests for api/auth/info (POST) endpoint.
/// </summary>
public class UpdateInfoTests : BaseTests
{
    private readonly UpdateUserInfoRequest _request = new()
    {
        Name = "Updated Name",
        Street = "123 Main",
        City = "Metro",
        State = "State",
        Country = "PH",
        PostalCode = 1111
    };

    public UpdateInfoTests()
    {
        Controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "user-1")], "mock"));
    }

    [Fact]
    public async Task ReturnsOk_WhenUserInfoUpdated()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.UpdateInfoAsync("user-1", _request))
            .ReturnsAsync(new Response
            {
                IsSuccess = true,
                Message = Messages.UserInfoUpdated
            });

        // Act
        var result = await Controller.UpdateInfo(_request);

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.UpdateInfoAsync("user-1", _request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Invalid input",
                ErrorType = ErrorType.Validation
            });

        // Act
        var result = await Controller.UpdateInfo(_request);

        // Assert
        var bad = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        bad.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.UpdateInfoAsync("user-1", _request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "User not found",
                ErrorType = ErrorType.NotFound
            });

        // Act
        var result = await Controller.UpdateInfo(_request);

        // Assert
        var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenSomethingFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.UpdateInfoAsync("user-1", _request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Something failed",
                ErrorType = ErrorType.Internal
            });

        // Act
        var result = await Controller.UpdateInfo(_request);

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}