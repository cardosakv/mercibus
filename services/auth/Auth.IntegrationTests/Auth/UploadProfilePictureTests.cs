using System.Net;
using System.Net.Http.Headers;
using Auth.Application.Common;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.IntegrationTests.Auth;

public class UploadProfilePictureTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenProfilePictureSuccessfullyUploaded()
    {
        // Arrange
        var user = new User
        {
            Email = "uploadtest@email.com",
            UserName = "upload_user",
            Name = "Uploader"
        };

        await UserManager.CreateAsync(user, "UploadPass@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);
        var token = await TestUtils.LoginUser(HttpClient, LoginUrl, user.UserName, "UploadPass@123");
        var form = TestUtils.CreateDummyImageForm();

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token!.AccessToken);
        var response = await HttpClient.PostAsync(UploadProfilePictureUrl, form);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var scopedUserManager = ServiceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var updatedUser = await scopedUserManager.FindByIdAsync(user.Id);
        updatedUser!.ProfileImageUrl.Should().NotBeNullOrEmpty();
        updatedUser.ProfileImageUrl.Should().Contain(Constants.BlobStorageContainerName);
    }

    [Fact]
    public async Task ReturnsUnauthorized_WhenNoTokenProvided()
    {
        // Arrange
        var form = TestUtils.CreateDummyImageForm();

        // Act
        var response = await HttpClient.PostAsync(UploadProfilePictureUrl, form);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}