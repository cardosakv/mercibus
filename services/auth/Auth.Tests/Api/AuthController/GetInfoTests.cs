using System.Security.Claims;
using Auth.Application.DTOs;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Mercibus.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

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
        var userInfo = new GetUserInfoResponse
        {
            Id = "user-1",
            Name = "Test User",
            Email = "test@email.com",
            Username = "test_user",
            IsEmailVerified = true,
            Street = "123 St",
            City = "Metro City",
            State = "Metro State",
            Country = "Ph",
            PostalCode = 1100
        };

        var response = new ApiSuccessResponse
        {
            Data = userInfo
        };

        AuthServiceMock
            .Setup(x => x.GetInfoAsync("user-1"))
            .ReturnsAsync(new ServiceResult { IsSuccess = true, Data = userInfo });

        // Act
        var result = await Controller.GetInfo();

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.StatusCode.Should().Be(StatusCodes.Status200OK);
        ok.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUserDoesNotExist()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.GetInfoAsync("user-1"))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.InvalidRequestError,
                ErrorCode = ErrorCode.UserNotFound
            });

        // Act
        var result = await Controller.GetInfo();

        // Assert
        var notFound = result.Should().BeOfType<ObjectResult>().Subject;
        notFound.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.GetInfoAsync("user-1"))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.ApiError,
                ErrorCode = ErrorCode.Internal
            });

        // Act
        var result = await Controller.GetInfo();

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}