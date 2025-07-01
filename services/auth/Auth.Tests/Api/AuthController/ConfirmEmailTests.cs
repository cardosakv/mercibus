using Auth.Application.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Tests for api/auth/confirm-email endpoint.
/// </summary>
public class ConfirmEmailTests : BaseTests
{
    private readonly string _userId = "user-1";
    private readonly string _token = "encoded-token";

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
            .Setup(x => x.ConfirmEmailAsync(_userId, _token))
            .ReturnsAsync(new Response { IsSuccess = true });

        // Act
        var result = await Controller.ConfirmEmail(_userId, _token);

        // Assert
        var redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be("https://success");
    }

    [Fact]
    public async Task RedirectsToFail_WhenConfirmationFails()
    {
        // Arrange
        AuthServiceMock
            .Setup(x => x.ConfirmEmailAsync(_userId, _token))
            .ReturnsAsync(new Response
            {
                IsSuccess = false,
                Message = "Invalid or expired token",
                ErrorType = ErrorType.BadRequest
            });

        // Act
        var result = await Controller.ConfirmEmail(_userId, _token);

        // Assert
        var redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be("https://fail");
    }
}