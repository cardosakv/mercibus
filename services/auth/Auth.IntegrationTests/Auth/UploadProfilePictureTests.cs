using System.Net;
using Auth.Application.Common;
using Auth.Domain.Common;
using Auth.IntegrationTests.Common;
using FluentAssertions;

namespace Auth.IntegrationTests.Auth;

public class UploadProfilePictureTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task ReturnsOk_WhenProfilePictureSuccessfullyUploaded()
    {
        // Arrange
        var user = await CreateUserAsync("upload_user", "uploadtest@email.com", roleName: Roles.Customer);
        var token = await LoginAsync(user.UserName!, "Test@123");
        var form = TestUtils.CreateDummyImageForm();

        // Act
        SetAuthorizationToken(token?.AccessToken);
        var response = await HttpClient.PostAsync(UploadProfilePictureUrl, form);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Reload to ensure fresh state
        await RefreshEntityAsync(user);
        user.ProfileImageUrl.Should().NotBeNullOrEmpty();
        user.ProfileImageUrl.Should().Contain(Constants.BlobStorageContainerName);
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