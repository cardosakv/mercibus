using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Tests for api/auth/change-password endpoint.
/// </summary>
public class ChangePasswordTests : BaseTests
{
    private readonly ChangePasswordRequest _request = new()
    {
        UserId = "user-1",
        CurrentPassword = "OldPassword123!",
        NewPassword = "NewPassword456!"
    };

    [Fact]
    public async Task ReturnsOk_WhenPasswordChangeSucceeds()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ChangePasswordAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = true,
                Message = Messages.PasswordChanged
            });

        // Act
        var result = await Controller.ChangePassword(_request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ChangePasswordAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Password too short",
                ErrorType = ErrorType.Validation
            });

        // Act
        var result = await Controller.ChangePassword(_request);

        // Assert
        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ChangePasswordAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "User not found",
                ErrorType = ErrorType.NotFound
            });

        // Act
        var result = await Controller.ChangePassword(_request);

        // Assert
        var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenSomethingGoesWrong()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ChangePasswordAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Unexpected error",
                ErrorType = ErrorType.Internal
            });

        // Act
        var result = await Controller.ChangePassword(_request);

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}