using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;

namespace Catalog.UnitTests.Application.ProductServiceTests;

public class UpdateProductAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenProductIsUpdated()
    {
        // Arrange
        long productId = 1;
        var request = new UpdateProductRequest(
            Name: "Updated Product",
            Description: "Updated Desc",
            Sku: "SKU-UPD",
            Price: 250.00m,
            StockQuantity: 20,
            Status: "listed",
            CategoryId: 1,
            BrandId: 2
        );

        var existingProduct = new Product
        {
            Id = productId,
            Name = "Old Product",
            Description = "Old Desc",
            Sku = "SKU-OLD",
            Price = 100,
            StockQuantity = 10,
            CategoryId = 1,
            BrandId = 2,
            CreatedAt = DateTime.UtcNow,
            Category = new Category { Id = 1, Name = "Category 1" },
            Brand = new Brand { Id = 2, Name = "Brand 2" }
        };

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);
        MapperMock.Setup(m => m.Map(request, existingProduct));
        ProductRepositoryMock.Setup(r => r.UpdateProductAsync(existingProduct, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await ProductService.UpdateProductAsync(productId, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Fail_WhenProductDoesNotExist()
    {
        // Arrange
        long productId = 999;
        var request = new UpdateProductRequest(
            Name: "Does Not Exist",
            Description: null,
            Sku: "NONE",
            Price: 0,
            StockQuantity: 0,
            Status: "listed",
            CategoryId: 1,
            BrandId: 1
        );

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await ProductService.UpdateProductAsync(productId, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        result.ErrorCode.Should().Be(Constants.ErrorCode.ProductNotFound);
    }

    [Fact]
    public async Task Throws_WhenUpdateFails()
    {
        // Arrange
        long productId = 123;
        var request = new UpdateProductRequest(
            Name: "Error Product",
            Description: "Something bad",
            Sku: "ERR",
            Price: 1,
            StockQuantity: 2,
            Status: "listed",
            CategoryId: 1,
            BrandId: 1
        );

        var existingProduct = new Product
        {
            Id = productId,
            Name = "Existing",
            Description = "Existing",
            Sku = "EX",
            Price = 1,
            StockQuantity = 1,
            CategoryId = 1,
            BrandId = 1,
            Category = new Category { Id = 1, Name = "C" },
            Brand = new Brand { Id = 1, Name = "B" },
            CreatedAt = DateTime.UtcNow
        };

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);
        MapperMock.Setup(m => m.Map(request, existingProduct));
        ProductRepositoryMock
            .Setup(r => r.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Update failed"));

        // Act
        Func<Task> act = async () => await ProductService.UpdateProductAsync(productId, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Update failed");
    }
}