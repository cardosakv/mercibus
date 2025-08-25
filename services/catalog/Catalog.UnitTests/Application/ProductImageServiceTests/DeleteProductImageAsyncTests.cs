using Catalog.Application.Common;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.ProductImageServiceTests;

public class DeleteProductImageAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenProductImageIsDeleted()
    {
        // Arrange
        var productId = 10;
        var imageId = 100;

        var product = new Product { Id = productId, Name = "Camera", Sku = "CAM-001" };
        var productImage = new ProductImage
        {
            Id = imageId,
            ProductId = productId,
            ImageUrl = $"{Constants.BlobStorage.ProductImagesContainer}/cam001.png"
        };

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        ProductImageRepositoryMock
            .Setup(r => r.GetProductImageByIdAsync(imageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productImage);
        BlobStorageServiceMock
            .Setup(b => b.DeleteBlobAsync(productImage.ImageUrl))
            .ReturnsAsync(true);
        ProductImageRepositoryMock
            .Setup(r => r.DeleteProductImageAsync(productImage, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        DbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await ProductImageService.DeleteProductImageAsync(productId, imageId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        BlobStorageServiceMock.Verify(b => b.DeleteBlobAsync(productImage.ImageUrl), Times.Once);
        ProductImageRepositoryMock.Verify(r => r.DeleteProductImageAsync(productImage, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReturnsError_WhenProductNotFound()
    {
        // Arrange
        var productId = 9999;
        var imageId = 1;

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await ProductImageService.DeleteProductImageAsync(productId, imageId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be(Constants.ErrorCode.ProductNotFound);
    }

    [Fact]
    public async Task ReturnsError_WhenImageNotFound()
    {
        // Arrange
        var productId = 10;
        var imageId = 999;

        var product = new Product { Id = productId, Name = "Phone", Sku = "PHN-200" };

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        ProductImageRepositoryMock
            .Setup(r => r.GetProductImageByIdAsync(imageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductImage?)null);

        // Act
        var result = await ProductImageService.DeleteProductImageAsync(productId, imageId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be(Constants.ErrorCode.ImageNotFound);
    }

    [Fact]
    public async Task ReturnsError_WhenImageDoesNotBelongToProduct()
    {
        // Arrange
        var productId = 10;
        var imageId = 100;

        var product = new Product { Id = productId, Name = "Laptop", Sku = "LAP-001" };
        var productImage = new ProductImage
        {
            Id = imageId,
            ProductId = 777, // different product
            ImageUrl = $"{Constants.BlobStorage.ProductImagesContainer}/wrong.png"
        };

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        ProductImageRepositoryMock
            .Setup(r => r.GetProductImageByIdAsync(imageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productImage);

        // Act
        var result = await ProductImageService.DeleteProductImageAsync(productId, imageId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be(Constants.ErrorCode.ImageNotInProduct);
    }

    [Fact]
    public async Task Throws_WhenBlobDeleteFails()
    {
        // Arrange
        var productId = 20;
        var imageId = 200;

        var product = new Product { Id = productId, Name = "Tablet", Sku = "TAB-300" };
        var productImage = new ProductImage
        {
            Id = imageId,
            ProductId = productId,
            ImageUrl = $"{Constants.BlobStorage.ProductImagesContainer}/tab.png"
        };

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        ProductImageRepositoryMock
            .Setup(r => r.GetProductImageByIdAsync(imageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productImage);

        BlobStorageServiceMock
            .Setup(b => b.DeleteBlobAsync(productImage.ImageUrl))
            .ThrowsAsync(new InvalidOperationException("Blob delete failed"));

        // Act
        Func<Task> act = async () => await ProductImageService.DeleteProductImageAsync(productId, imageId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Blob delete failed");
    }

    [Fact]
    public async Task Success_WhenSaveChangesReturnsZero()
    {
        // Arrange
        var productId = 30;
        var imageId = 300;

        var product = new Product { Id = productId, Name = "Monitor", Sku = "MON-100" };
        var productImage = new ProductImage
        {
            Id = imageId,
            ProductId = productId,
            ImageUrl = $"{Constants.BlobStorage.ProductImagesContainer}/monitor.png"
        };

        ProductRepositoryMock
            .Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        ProductImageRepositoryMock
            .Setup(r => r.GetProductImageByIdAsync(imageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productImage);
        BlobStorageServiceMock
            .Setup(b => b.DeleteBlobAsync(productImage.ImageUrl))
            .ReturnsAsync(It.IsAny<bool>());
        ProductImageRepositoryMock
            .Setup(r => r.DeleteProductImageAsync(productImage, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        DbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0); // edge case

        // Act
        var result = await ProductImageService.DeleteProductImageAsync(productId, imageId);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}