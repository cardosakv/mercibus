using System.Net;
using System.Net.Http.Json;
using Auth.Application.DTOs;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Newtonsoft.Json;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.IntegrationTests.Auth;

public class RefreshTokenTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenRefreshTokenIsValid()
    {
        // Arrange
        var user = new User { UserName = "refresh_user", Email = "refresh@email.com" };
        await UserManager.CreateAsync(user, "Valid@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var authToken = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, "Valid@123");
        var refreshRequest = new RefreshRequest { RefreshToken = authToken!.RefreshToken };

        // Act
        var response = await HttpClient.PostAsJsonAsync(RefreshTokenUrl, refreshRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var token = JsonConvert.DeserializeObject<AuthTokenResponse>(content.Data!.ToString()!);
        token.Should().NotBeNull();
        token!.AccessToken.Should().NotBeNullOrWhiteSpace();
        token.RefreshToken.Should().NotBeNullOrWhiteSpace();
        token.ExpiresIn.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenTokenNotValid()
    {
        // Arrange
        var request = new RefreshRequest { RefreshToken = "ghost-token" };

        // Act
        var response = await HttpClient.PostAsJsonAsync(RefreshTokenUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(ErrorCode.TokenInvalid);
    }
}