using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Tests for api/auth/reset-password endpoint.
/// </summary>
public class ResetPasswordTests : BaseTests
{
    private readonly ResetPasswordRequest _request = new()
    {
        UserId = "user-1",
        Token = "encoded-token",
        NewPassword = "NewPassword123!"
    };

    [Fact]
    public async Task ReturnsOk_WhenPasswordResetSucceeds()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ResetPasswordAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = true,
                Message = Messages.PasswordResetSuccess
            });

        // Act
        var result = await Controller.ResetPassword(_request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ResetPasswordAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Password does not meet complexity requirements",
                ErrorType = ErrorType.Validation
            });

        // Act
        var result = await Controller.ResetPassword(_request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ResetPasswordAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "User not found",
                ErrorType = ErrorType.NotFound
            });

        // Act
        var result = await Controller.ResetPassword(_request);

        // Assert
        var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ResetPasswordAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Something went wrong",
                ErrorType = ErrorType.Internal
            });

        // Act
        var result = await Controller.ResetPassword(_request);

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}