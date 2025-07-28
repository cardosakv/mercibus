using Catalog.Application.DTOs;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Catalog.Tests.Api.ProductController;

public class AddProductAsyncTests : BaseTest
{
    private static AddProductRequest SampleRequest => new(
        Name: "New Product",
        Description: "New Product Description",
        Sku: "SKU-NEW",
        Price: 100,
        StockQuantity: 10,
        Status: "listed",
        CategoryId: 1,
        BrandId: 1
    );

    [Fact]
    public async Task Returns_201Created_WhenProductIsAdded()
    {
        // Arrange
        var result = new ServiceResult
        {
            IsSuccess = true
        };

        ProductServiceMock
            .Setup(x => x.AddProductAsync(SampleRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.AddProductAsync(SampleRequest, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Returns_500InternalServerError_WhenUnhandledError()
    {
        // Arrange
        var result = new ServiceResult
        {
            IsSuccess = false,
            ErrorType = ErrorType.ApiError,
            ErrorCode = ErrorCode.Internal
        };

        ProductServiceMock
            .Setup(x => x.AddProductAsync(SampleRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.AddProductAsync(SampleRequest, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<ObjectResult>();
        var serverError = (ObjectResult)actionResult;
        serverError.StatusCode.Should().Be(500);
    }
}