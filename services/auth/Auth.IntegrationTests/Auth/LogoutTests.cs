using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Auth.Application.DTOs;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.IntegrationTests.Common;
using FluentAssertions;

namespace Auth.IntegrationTests.Auth;

public class LogoutTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenRefreshTokenIsValid()
    {
        // Arrange
        var user = new User { UserName = "logout_user", Email = "logout@email.com" };
        await UserManager.CreateAsync(user, "Valid@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var token = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, "Valid@123");
        var logoutRequest = new LogoutRequest { RefreshToken = token!.RefreshToken };

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        var response = await HttpClient.PostAsJsonAsync(LogoutUrl, logoutRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ReturnsUnauthorized_WhenTokenIsMissing()
    {
        // Arrange
        var request = new LogoutRequest { RefreshToken = "" };

        // Act
        var response = await HttpClient.PostAsJsonAsync(LogoutUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ReturnsUnauthorized_WhenRefreshTokenIsInvalid()
    {
        // Arrange
        const string invalidToken = "invalid-token";
        var logoutRequest = new LogoutRequest { RefreshToken = invalidToken };

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", invalidToken);
        var response = await HttpClient.PostAsJsonAsync(LogoutUrl, logoutRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}