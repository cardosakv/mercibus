using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Auth.Application.DTOs;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.IntegrationTests.Auth;

public class ChangePasswordTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenPasswordChangeSuccessful()
    {
        // Arrange
        var user = new User { UserName = "change_user", Email = "user@email.com", EmailConfirmed = true };
        const string originalPassword = "Original@123";
        const string newPassword = "NewStrong@456";

        await UserManager.CreateAsync(user, originalPassword);
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var request = new ChangePasswordRequest
        {
            UserId = user.Id,
            CurrentPassword = originalPassword,
            NewPassword = newPassword
        };

        var authToken = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, originalPassword);

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken!.AccessToken);
        var response = await HttpClient.PostAsJsonAsync(ChangePasswordUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUserNotFound()
    {
        // Arrange
        var user = new User { UserName = "not_found_user", Email = "user@email.com", EmailConfirmed = true };
        await UserManager.CreateAsync(user, "Valid@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var request = new ChangePasswordRequest
        {
            UserId = "non-existent-id",
            CurrentPassword = "DoesNotMatter@123",
            NewPassword = "NewPassword@123"
        };

        var authToken = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, "Valid@123");

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken!.AccessToken);
        var response = await HttpClient.PostAsJsonAsync(ChangePasswordUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(ErrorCode.UserNotFound);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenCurrentPasswordIsIncorrect()
    {
        // Arrange
        var user = new User { UserName = "wrong_pass_user", Email = "wrong@pass.com", EmailConfirmed = true };
        await UserManager.CreateAsync(user, "Correct@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var request = new ChangePasswordRequest
        {
            UserId = user.Id,
            CurrentPassword = "Wrong@123",
            NewPassword = "NewPassword@123"
        };

        var authToken = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, "Correct@123");

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken!.AccessToken);
        var response = await HttpClient.PostAsJsonAsync(ChangePasswordUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(ErrorCode.PasswordMismatch);
    }
}