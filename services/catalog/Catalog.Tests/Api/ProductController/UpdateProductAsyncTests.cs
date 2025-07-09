using Catalog.Application.Common;
using Catalog.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Catalog.Tests.Api.ProductController;

public class UpdateProductAsyncTests : BaseTest
{
    private static readonly UpdateProductRequest SampleRequest = new(
        Name: "Updated Name",
        Description: "Updated Description",
        Sku: "UPDATED-SKU",
        Price: 120,
        StockQuantity: 20,
        Status: "listed",
        CategoryId: 1,
        BrandId: 1
    );

    [Fact]
    public async Task Returns_204NoContent_WhenUpdateSucceeds()
    {
        // Arrange
        var result = new Result
        {
            IsSuccess = true
        };

        ProductServiceMock
            .Setup(x => x.UpdateProductAsync(1, SampleRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.UpdateProductAsync(1, SampleRequest, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<NoContentResult>();
        ((NoContentResult)actionResult).StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task Returns_404NotFound_WhenProductNotFound()
    {
        // Arrange
        var result = new Result
        {
            IsSuccess = false,
            ErrorType = ErrorType.NotFound,
            Message = "Product not found"
        };

        ProductServiceMock
            .Setup(x => x.UpdateProductAsync(999, SampleRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.UpdateProductAsync(999, SampleRequest, CancellationToken.None);

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
            Message = "Unexpected failure"
        };

        ProductServiceMock
            .Setup(x => x.UpdateProductAsync(1, SampleRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.UpdateProductAsync(1, SampleRequest, CancellationToken.None);

        // Assert
        var objectResult = actionResult.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().BeEquivalentTo(new StandardResponse { Message = "Unexpected failure" });
    }
}