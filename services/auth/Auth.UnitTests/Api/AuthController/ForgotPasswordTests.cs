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
/// Tests for api/auth/forgot-password endpoint.
/// </summary>
public class ForgotPasswordTests : BaseTests
{
    private readonly ForgotPasswordRequest _request = new()
    {
        Email = "test@email.com"
    };

    [Fact]
    public async Task ReturnsOk_WhenResetLinkSent()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ForgotPasswordAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = true
            });

        // Act
        var result = await Controller.ForgotPassword(_request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUserDoesNotExist()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ForgotPasswordAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.InvalidRequestError,
                ErrorCode = ErrorCode.UserNotFound
            });

        // Act
        var result = await Controller.ForgotPassword(_request);

        // Assert
        var notFoundResult = result.Should().BeOfType<ObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ForgotPasswordAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.ApiError,
                ErrorCode = ErrorCode.Internal
            });

        // Act
        var result = await Controller.ForgotPassword(_request);

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}