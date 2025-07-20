using System.Net;
using System.Text;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;

namespace Auth.IntegrationTests.Auth;

public class ConfirmEmailTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task RedirectsToSuccessUrl_WhenEmailIsConfirmed()
    {
        // Arrange
        var user = new User { UserName = "confirm_user_ok", Email = "confirm@email.com" };
        await UserManager.CreateAsync(user, "Valid@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var confirmLink = ConfirmEmailUrl + "?userId=" + user.Id + "&token=" + encodedToken;

        // Act
        var response = await HttpClient.GetAsync(confirmLink);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Be(Configuration["RedirectUrl:EmailConfirmSuccess"]);
    }

    [Fact]
    public async Task RedirectsToFailUrl_WhenUserNotFound()
    {
        // Arrange
        const string confirmLink = ConfirmEmailUrl + "?userId=" + "confirm_user_fail" + "&token=" + "invalid_token";

        // Act
        var response = await HttpClient.GetAsync(confirmLink);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Be(Configuration["RedirectUrl:EmailConfirmFail"]);
    }
}