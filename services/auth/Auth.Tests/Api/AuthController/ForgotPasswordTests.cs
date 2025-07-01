using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.Tests.Api.AuthController;

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
            .ReturnsAsync(new Response
            {
                IsSuccess = true,
                Message = Messages.PasswordResetLinkSent
            });

        // Act
        var result = await Controller.ForgotPassword(_request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ForgotPasswordAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "User not found",
                ErrorType = ErrorType.NotFound
            });

        // Act
        var result = await Controller.ForgotPassword(_request);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ForgotPasswordAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Internal failure",
                ErrorType = ErrorType.Internal
            });

        // Act
        var result = await Controller.ForgotPassword(_request);

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}