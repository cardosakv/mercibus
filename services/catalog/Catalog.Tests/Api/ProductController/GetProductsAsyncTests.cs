using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Catalog.Tests.Api.ProductController;

public class GetProductsAsyncTests : BaseTest
{
    private static GetProductsQuery SampleQuery => new(
        CategoryId: null,
        BrandId: null,
        MinPrice: null,
        MaxPrice: null,
        Status: null
    );

    [Fact]
    public async Task Returns_200Ok_WhenProductsExist()
    {
        // Arrange
        var response = new List<ProductResponse>
        {
            new(
                Id: 1,
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
            )
        };

        var result = new Result
        {
            IsSuccess = true,
            Data = response
        };

        ProductServiceMock
            .Setup(x => x.GetProductsAsync(SampleQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.GetProductsAsync(SampleQuery, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)actionResult;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Returns_404NotFound_WhenNoProductsFound()
    {
        // Arrange
        var result = new Result
        {
            IsSuccess = false,
            ErrorType = ErrorType.NotFound,
            Message = "No products found"
        };

        ProductServiceMock
            .Setup(x => x.GetProductsAsync(SampleQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.GetProductsAsync(SampleQuery, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)actionResult;
        notFound.StatusCode.Should().Be(404);
        notFound.Value.Should().BeEquivalentTo(new StandardResponse { Message = "No products found" });
    }

    [Fact]
    public async Task Returns_500InternalServerError_WhenUnhandledErrorOccurs()
    {
        // Arrange
        var result = new Result
        {
            IsSuccess = false,
            ErrorType = ErrorType.Internal,
            Message = "Unexpected failure"
        };

        ProductServiceMock
            .Setup(x => x.GetProductsAsync(SampleQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.GetProductsAsync(SampleQuery, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)actionResult;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().BeEquivalentTo(new StandardResponse { Message = "Unexpected failure" });
    }
}