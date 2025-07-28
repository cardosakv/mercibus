using Catalog.Application.Common;
using Catalog.Application.DTOs;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
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
            StockQuantity: 100,
            Brand: new BrandResponse(1, "Brand"),
            Category: new CategoryResponse(1, "Category", null),
            Images: [],
            Attributes: [],
            CreatedAt: DateTime.UtcNow
        );

        var result = new ServiceResult
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
    }

    [Fact]
    public async Task Returns_404NotFound_WhenProductDoesNotExist()
    {
        // Arrange
        long productId = 404;

        var result = new ServiceResult
        {
            IsSuccess = false,
            ErrorType = ErrorType.InvalidRequestError,
            ErrorCode = Constants.ErrorCode.ProductNotFound
        };

        ProductServiceMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.GetProductByIdAsync(productId, CancellationToken.None);

        // Assert
        var notFound = actionResult.Should().BeOfType<ObjectResult>().Subject;
        notFound.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Returns_500InternalServerError_WhenUnhandledErrorOccurs()
    {
        // Arrange
        long productId = 999;

        var result = new ServiceResult
        {
            IsSuccess = false,
            ErrorType = ErrorType.ApiError,
            ErrorCode = ErrorCode.Internal
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
    }
}