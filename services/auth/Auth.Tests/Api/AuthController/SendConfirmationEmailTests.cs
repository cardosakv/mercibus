using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Tests for api/auth/send-confirmation-email endpoint.
/// </summary>
public class SendConfirmationEmailTests : BaseTests
{
    private readonly SendConfirmationEmailRequest _request = new()
    {
        Email = "test@email.com"
    };

    [Fact]
    public async Task ReturnsOk_WhenEmailSentSuccessfully()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.SendConfirmationEmailAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = true,
                Message = Messages.EmailConfirmationSent
            });

        // Act
        var result = await Controller.SendConfirmationEmail(_request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.SendConfirmationEmailAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "User not found",
                ErrorType = ErrorType.NotFound
            });

        // Act
        var result = await Controller.SendConfirmationEmail(_request);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ReturnsConflict_WhenEmailAlreadyVerified()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.SendConfirmationEmailAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Email already verified",
                ErrorType = ErrorType.Conflict
            });

        // Act
        var result = await Controller.SendConfirmationEmail(_request);

        // Assert
        var conflict = result.Should().BeOfType<ConflictObjectResult>().Subject;
        conflict.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.SendConfirmationEmailAsync(_request))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Something went wrong",
                ErrorType = ErrorType.Internal
            });

        // Act
        var result = await Controller.SendConfirmationEmail(_request);

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}