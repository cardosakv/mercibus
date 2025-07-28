using System.Net;
using System.Net.Http.Json;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.IntegrationTests.Auth;

public class ForgotPasswordTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenForgotPasswordIsSuccessful()
    {
        // Arrange
        var user = new User { UserName = "reset_user", Email = "resetme@email.com" };
        await UserManager.CreateAsync(user, "Valid@123");

        var request = new ForgotPasswordRequest
        {
            Email = user.Email
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(ForgotPasswordUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenEmailNotFound()
    {
        // Arrange
        var request = new ForgotPasswordRequest
        {
            Email = "nonexistent@email.com"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(ForgotPasswordUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(ErrorCode.UserNotFound);
    }
}