using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Enums;
using FluentAssertions;
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
        var result = new Result
        {
            IsSuccess = true,
            ResourceId = 123,
            Message = "Product added successfully"
        };

        ProductServiceMock
            .Setup(x => x.AddProductAsync(SampleRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await ProductController.AddProductAsync(SampleRequest, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<CreatedResult>();
        var created = (CreatedResult)actionResult;
        created.StatusCode.Should().Be(201);
        created.Location.Should().Be("123");
        created.Value.Should().BeEquivalentTo(new StandardResponse { Message = "Product added successfully" });
    }

    [Fact]
    public async Task Returns_500InternalServerError_WhenUnhandledError()
    {
        // Arrange
        var result = new Result
        {
            IsSuccess = false,
            ErrorType = ErrorType.Internal,
            Message = "Unexpected error"
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
        serverError.Value.Should().BeEquivalentTo(new StandardResponse { Message = "Unexpected error" });
    }
}