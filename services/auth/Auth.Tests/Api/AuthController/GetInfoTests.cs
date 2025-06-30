using System.Security.Claims;
using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Tests for api/auth/info (GET) endpoint.
/// </summary>
public class GetInfoTests : BaseTests
{
    public GetInfoTests()
    {
        Controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "user-1")], "mock"));
    }

    [Fact]
    public async Task ReturnsOk_WhenUserInfoRetrieved()
    {
        // Arrange
        var userInfo = new GetUserInfoResponse { Name = "Test User", Email = "test@email.com" };

        AuthServiceMock
            .Setup(x => x.GetInfoAsync("user-1"))
            .ReturnsAsync(new Response { IsSuccess = true, Data = userInfo });

        // Act
        var result = await Controller.GetInfo();

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.StatusCode.Should().Be(StatusCodes.Status200OK);
        ok.Value.Should().BeEquivalentTo(userInfo);
    }

    [Fact]
    public async Task ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.GetInfoAsync("user-1"))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "User not found",
                ErrorType = ErrorType.NotFound
            });

        // Act
        var result = await Controller.GetInfo();

        // Assert
        var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.GetInfoAsync("user-1"))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Unexpected error",
                ErrorType = ErrorType.Internal
            });

        // Act
        var result = await Controller.GetInfo();

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}