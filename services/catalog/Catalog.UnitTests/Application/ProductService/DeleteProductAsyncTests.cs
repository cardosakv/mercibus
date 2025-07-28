using Catalog.Application.Common;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;

namespace Catalog.Tests.Application.ProductService;

public class DeleteProductAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenProductIsDeleted()
    {
        // Arrange
        long productId = 1;
        var product = new Product
        {
            Id = productId,
            Name = "Delete Me",
            Sku = "DEL-001",
            Status = ProductStatus.Listed,
            CategoryId = 1,
            BrandId = 2,
            Price = 100,
            StockQuantity = 5,
            CreatedAt = DateTime.UtcNow,
            Category = new Category { Id = 1, Name = "Cat" },
            Brand = new Brand { Id = 2, Name = "Brand" }
        };

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        ProductRepositoryMock
            .Setup(r => r.DeleteProductAsync(product, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        DbContextMock
            .Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await ProductService.DeleteProductAsync(productId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Fail_WhenProductDoesNotExist()
    {
        // Arrange
        long productId = 99;
        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await ProductService.DeleteProductAsync(productId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        result.ErrorCode.Should().Be(Constants.ErrorCode.ProductNotFound);
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrows()
    {
        // Arrange
        long productId = 123;
        var product = new Product
        {
            Id = productId,
            Name = "Explode",
            Sku = "ERR-123",
            Status = ProductStatus.Unlisted,
            CategoryId = 1,
            BrandId = 1,
            Price = 20,
            StockQuantity = 0,
            CreatedAt = DateTime.UtcNow,
            Category = new Category { Id = 1, Name = "C" },
            Brand = new Brand { Id = 1, Name = "B" }
        };

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        ProductRepositoryMock
            .Setup(r => r.DeleteProductAsync(product, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Failed to delete"));

        // Act
        Func<Task> act = async () => await ProductService.DeleteProductAsync(productId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Failed to delete");
    }
}