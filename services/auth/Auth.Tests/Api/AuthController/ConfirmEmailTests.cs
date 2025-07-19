using Auth.Application.DTOs;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Tests for api/auth/confirm-email endpoint.
/// </summary>
public class ConfirmEmailTests : BaseTests
{
    private readonly ConfirmEmailQuery _query = new()
    {
        UserId = "user-1",
        Token = "encoded-token"
    };

    public ConfirmEmailTests()
    {
        ConfigurationMock.Setup(x => x["RedirectUrl:EmailConfirmSuccess"]).Returns("https://success");
        ConfigurationMock.Setup(x => x["RedirectUrl:EmailConfirmFail"]).Returns("https://fail");
    }

    [Fact]
    public async Task RedirectsToSuccess_WhenEmailConfirmed()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ConfirmEmailAsync(_query))
            .ReturnsAsync(new ServiceResult { IsSuccess = true });

        // Act
        var result = await Controller.ConfirmEmail(_query);

        // Assert
        var redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be("https://success");
    }

    [Fact]
    public async Task RedirectsToFail_WhenConfirmationFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ConfirmEmailAsync(_query))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.InvalidRequestError,
                ErrorCode = ErrorCode.Internal
            });

        // Act
        var result = await Controller.ConfirmEmail(_query);

        // Assert
        var redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be("https://fail");
    }
}