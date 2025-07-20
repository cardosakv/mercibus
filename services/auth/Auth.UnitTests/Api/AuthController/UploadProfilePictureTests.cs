using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.UnitTests.Api.AuthController;

public class UploadProfilePictureTests : BaseTests
{
    [Fact]
    public async Task ReturnsOk_WhenProfilePictureUploaded()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();

        AuthServiceMock
            .Setup(x => x.UploadProfilePictureAsync(It.IsAny<string>(), It.IsAny<IFormFile>()))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = true
            });

        // Act
        var result = await Controller.UploadProfilePicture(fileMock.Object);

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUserNotFound()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();

        AuthServiceMock
            .Setup(x => x.UploadProfilePictureAsync(It.IsAny<string>(), It.IsAny<IFormFile>()))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.InvalidRequestError,
                ErrorCode = ErrorCode.UserNotFound
            });

        // Act
        var result = await Controller.UploadProfilePicture(fileMock.Object);

        // Assert
        var notFound = result.Should().BeOfType<ObjectResult>().Subject;
        notFound.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ReturnsInternalServerError_WhenSomethingFails()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();

        AuthServiceMock
            .Setup(x => x.UploadProfilePictureAsync(It.IsAny<string>(), It.IsAny<IFormFile>()))
            .ReturnsAsync(new ServiceResult
            {
                IsSuccess = false,
                ErrorType = ErrorType.ApiError,
                ErrorCode = ErrorCode.Internal
            });

        // Act
        var result = await Controller.UploadProfilePicture(fileMock.Object);

        // Assert
        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}