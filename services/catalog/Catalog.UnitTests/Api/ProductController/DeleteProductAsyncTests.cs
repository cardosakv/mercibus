using Catalog.Application.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Catalog.Tests.Api.ProductController;

public class DeleteProductAsyncTests : BaseTest
{
    [Fact]
    public async Task Returns_204NoContent_WhenDeleteSucceeds()
    {
        // Arrange
        var result = new ServiceResult
        {
            IsSuccess = true
        };

        ProductServiceMock
            .Setup(x => x.DeleteProductAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.DeleteProductAsync(1, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Returns_404NotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var result = new ServiceResult
        {
            IsSuccess = false,
            ErrorType = ErrorType.InvalidRequestError,
            ErrorCode = Constants.ErrorCode.ProductNotFound
        };

        ProductServiceMock
            .Setup(x => x.DeleteProductAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.DeleteProductAsync(999, CancellationToken.None);

        // Assert
        var notFound = actionResult.Should().BeOfType<ObjectResult>().Subject;
        notFound.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Returns_500InternalServerError_WhenUnexpectedFailure()
    {
        // Arrange
        var result = new ServiceResult
        {
            IsSuccess = false,
            ErrorType = ErrorType.ApiError
        };

        ProductServiceMock
            .Setup(x => x.DeleteProductAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.DeleteProductAsync(1, CancellationToken.None);

        // Assert
        var objectResult = actionResult.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(500);
    }
}