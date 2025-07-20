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

public class UpdateInfoTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenUserInfoSuccessfullyUpdated()
    {
        // Arrange
        var user = new User
        {
            Email = "edituser@email.com",
            UserName = "edit_user",
            Name = "Old Name",
            PhoneNumber = "09170000000",
            Street = "Old St",
            City = "Old City",
            State = "Old State",
            PostalCode = 1000,
            Country = "Old Country",
            ProfileImageUrl = "https://old.example.com/profile.jpg",
            BirthDate = DateTime.SpecifyKind(new DateTime(2001, 12, 12), DateTimeKind.Utc)
        };

        await UserManager.CreateAsync(user, "EditPass@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var updateRequest = new UpdateUserInfoRequest
        {
            Name = "Updated Name",
            PhoneNumber = "09179999999",
            Street = "New Street",
            City = "New City",
            State = "New State",
            PostalCode = 6000,
            Country = "New Country",
            ProfileImageUrl = "https://new.example.com/profile.jpg",
            BirthDate = DateTime.SpecifyKind(new DateTime(2000, 01, 01), DateTimeKind.Utc)
        };

        var token = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, "EditPass@123");

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token!.AccessToken);
        var response = await HttpClient.PostAsJsonAsync(GetUpdateInfoUrl, updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var updatedInfo = JsonConvert.DeserializeObject<GetUserInfoResponse>(content.Data!.ToString()!);
        updatedInfo.Should().NotBeNull();
        updatedInfo!.Name.Should().Be(updateRequest.Name);
        updatedInfo.PhoneNumber.Should().Be(updateRequest.PhoneNumber);
        updatedInfo.Street.Should().Be(updateRequest.Street);
        updatedInfo.City.Should().Be(updateRequest.City);
        updatedInfo.State.Should().Be(updateRequest.State);
        updatedInfo.PostalCode.Should().Be(updateRequest.PostalCode);
        updatedInfo.Country.Should().Be(updateRequest.Country);
        updatedInfo.ProfileImageUrl.Should().Be(updateRequest.ProfileImageUrl);
        updatedInfo.BirthDate.Should().Be(updateRequest.BirthDate);
    }

    [Fact]
    public async Task ReturnsUnauthorized_WhenNoTokenProvided()
    {
        // Arrange
        var updateRequest = new UpdateUserInfoRequest
        {
            Name = "Should Fail"
        };

        var request = new HttpRequestMessage(HttpMethod.Post, GetUpdateInfoUrl)
        {
            Content = JsonContent.Create(updateRequest)
        };

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}