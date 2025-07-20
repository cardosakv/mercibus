using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Auth.Application.DTOs;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Auth.IntegrationTests.Auth;

public class GetInfoTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenUserInfoSuccessfullyRetrieved()
    {
        // Arrange
        var user = new User
        {
            Email = "user@email.com",
            UserName = "user123",
            Name = "Test User",
            PhoneNumber = "1234567890",
            Street = "123 Test St",
            City = "Test City",
            State = "Test State",
            PostalCode = 6000,
            Country = "Test Country",
            ProfileImageUrl = "https://example.com/profile.jpg",
            BirthDate = DateTime.MaxValue
        };

        await UserManager.CreateAsync(user, "StrongPass@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var token = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, "StrongPass@123");

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token!.AccessToken);
        var response = await HttpClient.GetAsync(GetUpdateInfoUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var userInfo = JsonConvert.DeserializeObject<GetUserInfoResponse>(content.Data!.ToString()!);
        userInfo.Should().NotBeNull();
        userInfo!.Username.Should().Be(user.UserName);
        userInfo.Email.Should().Be(user.Email);
        userInfo.Name.Should().Be(user.Name);
        userInfo.PhoneNumber.Should().Be(user.PhoneNumber);
        userInfo.Street.Should().Be(user.Street);
        userInfo.City.Should().Be(user.City);
        userInfo.State.Should().Be(user.State);
        userInfo.PostalCode.Should().Be(user.PostalCode);
        userInfo.Country.Should().Be(user.Country);
        userInfo.ProfileImageUrl.Should().Be(user.ProfileImageUrl);
        userInfo.BirthDate.Should().Be(user.BirthDate);
    }

    [Fact]
    public async Task ReturnsUnauthorized_WhenNoTokenProvided()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, GetUpdateInfoUrl);

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}