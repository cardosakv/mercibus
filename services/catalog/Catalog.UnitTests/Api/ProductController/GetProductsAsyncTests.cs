using Catalog.Application.DTOs;
using Catalog.Domain.Enums;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
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

        var result = new ServiceResult
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
    }

    [Fact]
    public async Task Returns_404NotFound_WhenNoProductsFound()
    {
        // Arrange
        var result = new ServiceResult
        {
            IsSuccess = false,
            ErrorType = ErrorType.InvalidRequestError
        };

        ProductServiceMock
            .Setup(x => x.GetProductsAsync(SampleQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.GetProductsAsync(SampleQuery, CancellationToken.None);

        // Assert
        var notFound = actionResult.Should().BeOfType<ObjectResult>().Subject;
        notFound.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Returns_500InternalServerError_WhenUnhandledErrorOccurs()
    {
        // Arrange
        var result = new ServiceResult
        {
            IsSuccess = false,
            ErrorType = ErrorType.ApiError,
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
    }
}