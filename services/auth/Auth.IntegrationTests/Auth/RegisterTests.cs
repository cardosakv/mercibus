using System.Net;
using System.Net.Http.Json;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Newtonsoft.Json;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.IntegrationTests.Auth;

public class RegisterTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenRegistrationSucceeds()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "test_user",
            Email = "test@email.com",
            Password = "Test@1234",
        };

        // Act
        var responseJson = await HttpClient.PostAsJsonAsync(RegisterUrl, request);

        // Assert
        responseJson.StatusCode.Should().Be(HttpStatusCode.OK);
        responseJson.Content.Should().NotBeNull();

        var response = await responseJson.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();

        var userInfo = JsonConvert.DeserializeObject<GetUserInfoResponse>(response.Data!.ToString()!);
        userInfo.Should().NotBeNull();
        userInfo!.Id.Should().NotBeNullOrEmpty();
        userInfo.Username.Should().Be(request.Username);
        userInfo.Email.Should().Be(request.Email);

        var userExists = DbContext.Users.Any(u => u.UserName == request.Username);
        userExists.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "us",
            Email = "incorrect_email.com",
            Password = "123",
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(RegisterUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        error.Should().NotBeNull();
        error!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        error.Error.Code.Should().Be(ErrorCode.ValidationFailed);
        error.Error.Params.Should().NotBeNullOrEmpty();
        error.Error.Params.Should().Contain(x => x.Field == "username" && x.Code == ErrorCode.UsernameTooShort);
        error.Error.Params.Should().Contain(x => x.Field == "email" && x.Code == ErrorCode.EmailInvalid);
        error.Error.Params.Should().Contain(x => x.Field == "password" && x.Code == ErrorCode.PasswordTooShort);
    }

    [Fact]
    public async Task ReturnsConflict_WhenUsernameAlreadyExists()
    {
        // Arrange: seed a user manually
        var existingUser = new User
        {
            UserName = "duplicate_user",
            Email = "duplicate@email.com",
        };

        await UserManager.CreateAsync(existingUser, "Test@12345");

        var request = new RegisterRequest
        {
            Username = "duplicate_user",
            Email = "new@email.com",
            Password = "Test@12345",
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(RegisterUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        error.Should().NotBeNull();
        error!.Error.Type.Should().Be(ErrorType.ConflictError);
        error.Error.Code.Should().Be(ErrorCode.UsernameAlreadyExists);
    }
}