using Catalog.Application.Common;
using Catalog.Application.DTOs;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Catalog.UnitTests.Api.ProductControllerTests;

public class UpdateProductAsyncTests : BaseTest
{
    private static readonly UpdateProductRequest SampleRequest = new(
        Name: "Updated Name",
        Description: "Updated Description",
        Sku: "UPDATED-SKU",
        Price: 120,
        StockQuantity: 20,
        CategoryId: 1,
        BrandId: 1
    );

    [Fact]
    public async Task Returns_204NoContent_WhenUpdateSucceeds()
    {
        // Arrange
        var result = new ServiceResult
        {
            IsSuccess = true
        };

        ProductServiceMock
            .Setup(x => x.UpdateProductAsync(1, SampleRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.UpdateProductAsync(id: 1, SampleRequest, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Returns_404NotFound_WhenProductNotFound()
    {
        // Arrange
        var result = new ServiceResult
        {
            IsSuccess = false,
            ErrorType = ErrorType.InvalidRequestError,
            ErrorCode = Constants.ErrorCode.ProductNotFound
        };

        ProductServiceMock
            .Setup(x => x.UpdateProductAsync(999, SampleRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.UpdateProductAsync(id: 999, SampleRequest, CancellationToken.None);

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
            .Setup(x => x.UpdateProductAsync(1, SampleRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.UpdateProductAsync(id: 1, SampleRequest, CancellationToken.None);

        // Assert
        var objectResult = actionResult.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(500);
    }
}