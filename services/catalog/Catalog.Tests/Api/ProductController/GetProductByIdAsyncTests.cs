using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Catalog.Tests.Api.ProductController;

public class GetProductByIdAsyncTests : BaseTest
{
    [Fact]
    public async Task Returns_200Ok_WhenProductExists()
    {
        // Arrange
        long productId = 1;

        var expectedProduct = new ProductResponse(
            Id: productId,
            Name: "Product 1",
            Description: "Desc",
            Price: 10,
            Sku: "SKU1",
            Status: ProductStatus.Listed,
            StockQuantity: 100,
            Brand: new BrandResponse(1, "Brand"),
            Category: new CategoryResponse(1, "Category", null),
            Images: [],
            Attributes: [],
            CreatedAt: DateTime.UtcNow
        );

        var result = new Result
        {
            IsSuccess = true,
            Data = expectedProduct
        };

        ProductServiceMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.GetProductByIdAsync(productId, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)actionResult;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task Returns_404NotFound_WhenProductDoesNotExist()
    {
        // Arrange
        long productId = 404;

        var result = new Result
        {
            IsSuccess = false,
            ErrorType = ErrorType.NotFound,
            Message = "Product not found"
        };

        ProductServiceMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.GetProductByIdAsync(productId, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)actionResult;
        notFound.StatusCode.Should().Be(404);
        notFound.Value.Should().BeEquivalentTo(new StandardResponse { Message = "Product not found" });
    }

    [Fact]
    public async Task Returns_500InternalServerError_WhenUnhandledErrorOccurs()
    {
        // Arrange
        long productId = 999;

        var result = new Result
        {
            IsSuccess = false,
            ErrorType = ErrorType.Internal,
            Message = "Unexpected error"
        };

        ProductServiceMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.GetProductByIdAsync(productId, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)actionResult;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().BeEquivalentTo(new StandardResponse { Message = "Unexpected error" });
    }
}