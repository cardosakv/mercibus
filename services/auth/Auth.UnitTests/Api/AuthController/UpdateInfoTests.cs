using System.Security.Claims;
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
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = true
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
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.InvalidRequestError,
                ErrorCode = ErrorCode.NameTooShort
            });

        // Act
        var result = await Controller.UpdateInfo(_request);

        // Assert
        var bad = result.Should().BeOfType<ObjectResult>().Subject;
        bad.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUserNotFound()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.UpdateInfoAsync("user-1", _request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.InvalidRequestError,
                ErrorCode = ErrorCode.UserNotFound
            });

        // Act
        var result = await Controller.UpdateInfo(_request);

        // Assert
        var notFound = result.Should().BeOfType<ObjectResult>().Subject;
        notFound.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenSomethingFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.UpdateInfoAsync("user-1", _request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.ApiError,
                ErrorCode = ErrorCode.Internal
            });

        // Act
        var result = await Controller.UpdateInfo(_request);

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}