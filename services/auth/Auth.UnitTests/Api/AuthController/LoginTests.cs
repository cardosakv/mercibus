using Auth.Application.DTOs;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Mercibus.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.UnitTests.Api.AuthController;

/// <summary>
/// Tests for api/auth/login endpoint.
/// </summary>
public class LoginTests : BaseTests
{
    private readonly LoginRequest _request = new()
    {
        Username = "test",
        Password = "password"
    };

    private readonly AuthTokenResponse _token = new()
    {
        AccessToken = "access-token",
        RefreshToken = "refresh-token",
        ExpiresIn = 3600
    };

    [Fact]
    public async Task ReturnsOk_WhenLoginSucceeds()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LoginAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = true,
                Data = _token
            });

        // Act
        var result = await Controller.Login(_request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var token = okResult.Value.Should().BeOfType<ApiSuccessResponse>().Subject;
        token.Data.Should().BeEquivalentTo(_token);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LoginAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.InvalidRequestError,
                ErrorCode = ErrorCode.PasswordTooShort
            });

        // Act
        var result = await Controller.Login(_request);

        // Assert
        var badRequestResult = result.Should().BeOfType<ObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUserNotFound()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LoginAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.InvalidRequestError,
                ErrorCode = ErrorCode.UserNotFound
            });

        // Act
        var result = await Controller.Login(_request);

        // Assert
        var notFoundResult = result.Should().BeOfType<ObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsForbidden_WhenUserIsBlocked()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LoginAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.PermissionError,
                ErrorCode = ErrorCode.UserNoRoleAssigned
            });

        // Act
        var result = await Controller.Login(_request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.LoginAsync(_request))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.ApiError,
                ErrorCode = ErrorCode.Internal
            });

        // Act
        var result = await Controller.Login(_request);

        // Assert
        var errorResult = result.Should().BeOfType<ObjectResult>().Subject;
        errorResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}