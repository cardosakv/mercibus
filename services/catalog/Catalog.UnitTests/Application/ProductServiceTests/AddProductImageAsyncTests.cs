using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Catalog.UnitTests.Application.ProductServiceTests;

public class AddProductImageAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenProductImageIsAdded()
    {
        // Arrange
        var productId = 10;
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("test.png");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream([1, 2, 3]));

        var request = new ProductImageRequest(mockFile.Object, IsPrimary: true, AltText: "Sample image");

        var product = new Product { Id = productId, Name = "Camera", Sku = "CAM-001" };
        ProductRepositoryMock.Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var mappedImage = new ProductImage
        {
            Id = 1,
            ProductId = productId,
            AltText = request.AltText,
            IsPrimary = request.IsPrimary,
            ImageUrl = $"{Constants.BlobStorage.ProductImagesContainer}/test.png"
        };

        MapperMock.Setup(m => m.Map<ProductImage>(request)).Returns(mappedImage);
        ProductRepositoryMock.Setup(r => r.AddProductImageAsync(mappedImage, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mappedImage);
        DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        BlobStorageServiceMock.Setup(b => b.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync(It.IsAny<string>());

        // Act
        var result = await ProductService.AddProductImageAsync(productId, request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        ProductRepositoryMock.Verify(r => r.AddProductImageAsync(mappedImage, It.IsAny<CancellationToken>()), Times.Once);
        BlobStorageServiceMock.Verify(b => b.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
    }

    [Fact]
    public async Task ReturnsError_WhenProductNotFound()
    {
        // Arrange
        var productId = 99;
        ProductRepositoryMock.Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("test.png");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        var request = new ProductImageRequest(mockFile.Object, IsPrimary: false, AltText: "Alt text");

        // Act
        var result = await ProductService.AddProductImageAsync(productId, request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be(Constants.ErrorCode.ProductNotFound);
    }

    [Fact]
    public async Task Throws_WhenUploadFails()
    {
        // Arrange
        var productId = 20;
        var product = new Product { Id = productId, Name = "Laptop", Sku = "LAP-100" };
        ProductRepositoryMock.Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("broken.jpg");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        var request = new ProductImageRequest(mockFile.Object, IsPrimary: false, AltText: "Broken upload");

        BlobStorageServiceMock.Setup(b => b.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ThrowsAsync(new InvalidOperationException("Upload failed"));

        // Act
        Func<Task> act = async () => await ProductService.AddProductImageAsync(productId, request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Upload failed");
    }

    [Fact]
    public async Task Success_WhenSaveChangesReturnsZero()
    {
        // Arrange
        var productId = 30;
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("save.png");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        var request = new ProductImageRequest(mockFile.Object, IsPrimary: false, AltText: "Edge case");

        var product = new Product { Id = productId, Name = "Phone", Sku = "PHN-200" };
        ProductRepositoryMock.Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var mappedImage = new ProductImage
        {
            Id = 5,
            ProductId = productId,
            AltText = request.AltText,
            IsPrimary = request.IsPrimary,
            ImageUrl = $"{Constants.BlobStorage.ProductImagesContainer}/save.png"
        };

        MapperMock.Setup(m => m.Map<ProductImage>(request)).Returns(mappedImage);
        ProductRepositoryMock.Setup(r => r.AddProductImageAsync(mappedImage, It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<ProductImage>());
        DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        BlobStorageServiceMock.Setup(b => b.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync(It.IsAny<string>());

        // Act
        var result = await ProductService.AddProductImageAsync(productId, request);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}