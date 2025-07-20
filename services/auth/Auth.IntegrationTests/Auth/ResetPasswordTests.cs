using System.Net;
using System.Net.Http.Json;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.IntegrationTests.Auth;

public class ResetPasswordTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenPasswordResetSuccessful()
    {
        // Arrange
        const string email = "resetme@email.com";
        var user = new User { UserName = "reset_user", Email = email, EmailConfirmed = true };
        await UserManager.CreateAsync(user, "OldPassword@123");

        var token = await UserManager.GeneratePasswordResetTokenAsync(user);

        var request = new ResetPasswordRequest
        {
            UserId = user.Id,
            Token = token,
            NewPassword = "NewValid@123"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(ResetPasswordUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUserNotFound()
    {
        // Arrange
        var request = new ResetPasswordRequest
        {
            UserId = "invalid-id",
            Token = "token",
            NewPassword = "NewValid@123"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(ResetPasswordUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(ErrorCode.UserNotFound);
    }
}