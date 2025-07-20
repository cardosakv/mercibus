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
using Newtonsoft.Json;

namespace Auth.IntegrationTests.Auth;

public class LoginTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenLoginIsSuccessful()
    {
        // Arrange: create and register a valid user
        var user = new User
        {
            UserName = "valid_user",
            Email = "valid@email.com"
        };
        await UserManager.CreateAsync(user, "Valid@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var request = new LoginRequest
        {
            Username = "valid_user",
            Password = "Valid@123"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(LoginUrl, request);

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
    public async Task ReturnsBadRequest_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "non_existing_user",
            Password = "SomePassword123"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(LoginUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new User { UserName = "wrong_pass_user", Email = "wrong@pass.com" };
        await UserManager.CreateAsync(user, "Correct@Password1");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var request = new LoginRequest
        {
            Username = "wrong_pass_user",
            Password = "WrongPassword"
        };

        var token = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, "Correct@Password1");

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token!.AccessToken);
        var response = await HttpClient.PostAsJsonAsync(LoginUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}