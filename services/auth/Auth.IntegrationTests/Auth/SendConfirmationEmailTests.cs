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

public class SendConfirmationEmailTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenEmailSentSuccessfully()
    {
        // Arrange
        var user = new User { UserName = "email_user", Email = "email@send.com", EmailConfirmed = false };
        await UserManager.CreateAsync(user, "Valid@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var request = new SendConfirmationEmailRequest { Email = user.Email };
        var authToken = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, "Valid@123");

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken!.AccessToken);
        var response = await HttpClient.PostAsJsonAsync(SendConfirmationEmailUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUserNotFound()
    {
        // Arrange
        var user = new User { UserName = "email_user", Email = "email@send.com", EmailConfirmed = false };
        await UserManager.CreateAsync(user, "Valid@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var request = new SendConfirmationEmailRequest { Email = "nonexistent@send.com" };
        var authToken = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, "Valid@123");

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken!.AccessToken);
        var response = await HttpClient.PostAsJsonAsync(SendConfirmationEmailUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(ErrorCode.UserNotFound);
    }
}