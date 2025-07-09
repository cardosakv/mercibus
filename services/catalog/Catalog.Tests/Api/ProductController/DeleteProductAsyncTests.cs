using Catalog.Application.Common;
using Catalog.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Catalog.Tests.Api.ProductController;

public class DeleteProductAsyncTests : BaseTest
{
    [Fact]
    public async Task Returns_204NoContent_WhenDeleteSucceeds()
    {
        // Arrange
        var result = new Result
        {
            IsSuccess = true
        };

        ProductServiceMock
            .Setup(x => x.DeleteProductAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.DeleteProductAsync(1, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<NoContentResult>();
        ((NoContentResult)actionResult).StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task Returns_404NotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var result = new Result
        {
            IsSuccess = false,
            ErrorType = ErrorType.NotFound,
            Message = "Product not found"
        };

        ProductServiceMock
            .Setup(x => x.DeleteProductAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.DeleteProductAsync(999, CancellationToken.None);

        // Assert
        var notFound = actionResult.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(404);
        notFound.Value.Should().BeEquivalentTo(new StandardResponse { Message = "Product not found" });
    }

    [Fact]
    public async Task Returns_500InternalServerError_WhenUnexpectedFailure()
    {
        // Arrange
        var result = new Result
        {
            IsSuccess = false,
            ErrorType = ErrorType.Internal,
            Message = "Unexpected error occurred"
        };

        ProductServiceMock
            .Setup(x => x.DeleteProductAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.DeleteProductAsync(1, CancellationToken.None);

        // Assert
        var objectResult = actionResult.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().BeEquivalentTo(new StandardResponse { Message = "Unexpected error occurred" });
    }
}