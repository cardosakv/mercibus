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
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = true
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
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.InvalidRequestError,
                ErrorCode = ErrorCode.PasswordTooShort
            });

        // Act
        var result = await Controller.ResetPassword(_request);

        // Assert
        var badRequestResult = result.Should().BeOfType<ObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUserNotFound()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ResetPasswordAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.InvalidRequestError,
                ErrorCode = ErrorCode.UserNotFound
            });

        // Act
        var result = await Controller.ResetPassword(_request);

        // Assert
        var notFound = result.Should().BeOfType<ObjectResult>().Subject;
        notFound.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ResetPasswordAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.ApiError,
                ErrorCode = ErrorCode.Internal
            });

        // Act
        var result = await Controller.ResetPassword(_request);

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}