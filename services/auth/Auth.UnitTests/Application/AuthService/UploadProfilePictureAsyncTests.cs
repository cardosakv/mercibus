using Auth.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Auth.UnitTests.Application.AuthService;

/// <summary>
/// Tests for auth service upload profile picture method.
/// </summary>
public class UploadProfilePictureAsyncTests : BaseTests
{
    [Fact]
    public async Task Success_WhenProfilePictureUploaded()
    {
        // Arrange
        var file = new Mock<IFormFile>();
        var user = new User { Id = "user-1" };

        UserManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        UserManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);
        BlobStorageServiceMock
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync("uploaded-image-url");

        // Act
        var response = await AuthService.UploadProfilePictureAsync(user.Id, file.Object);

        // Assert
        UserManagerMock.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
        UserManagerMock.Verify(x => x.UpdateAsync(It.Is<User>(u => u.ProfileImageUrl != null)), Times.Once);
        BlobStorageServiceMock.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);

        response.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        var file = new Mock<IFormFile>();

        UserManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.UploadProfilePictureAsync("non-existent-user", file.Object);

        // Assert
        UserManagerMock.Verify(x => x.FindByIdAsync("non-existent-user"), Times.Once);
        BlobStorageServiceMock.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()), Times.Never);

        response.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Fail_WhenUpdateFails()
    {
        // Arrange
        var file = new Mock<IFormFile>();
        var user = new User { Id = "user-1" };

        UserManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        UserManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));
        BlobStorageServiceMock
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync("uploaded-image-url");

        // Act
        var response = await AuthService.UploadProfilePictureAsync(user.Id, file.Object);

        // Assert
        UserManagerMock.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
        UserManagerMock.Verify(x => x.UpdateAsync(It.Is<User>(u => u.ProfileImageUrl != null)), Times.Once);
        BlobStorageServiceMock.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);

        response.IsSuccess.Should().BeFalse();
    }
}