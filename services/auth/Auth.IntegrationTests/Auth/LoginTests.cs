using System.Net;
using System.Net.Http.Json;
using Auth.Application.DTOs;
using Auth.Domain.Common;
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
        // Arrange
        var user = await CreateUserAsync("valid_user", "valid@email.com", "Valid@123", Roles.Customer);

        var request = new LoginRequest
        {
            Username = user.UserName,
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
        var user = await CreateUserAsync("wrong_pass_user", "wrong@pass.com", "Correct@Password1", Roles.Customer);

        var request = new LoginRequest
        {
            Username = user.UserName,
            Password = "WrongPassword"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(LoginUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}