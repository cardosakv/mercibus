using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;

namespace Catalog.UnitTests.Application.ProductServiceTests;

public class GetProductByIdAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenProductExists()
    {
        // Arrange
        long productId = 1;

        var productEntity = new Product
        {
            Id = productId,
            Name = "Sample Product",
            Description = "Description here",
            Price = 50,
            Sku = "SKU-001",
            StockQuantity = 100,
            CategoryId = 1,
            BrandId = 2,
            CreatedAt = DateTime.UtcNow,
            Category = new Category { Id = 1, Name = "Category 1" },
            Brand = new Brand { Id = 2, Name = "Brand 2" }
        };

        var responseDto = new ProductResponse(
            productId,
            productEntity.Name,
            productEntity.Description,
            productEntity.Price,
            productEntity.Sku,
            productEntity.StockQuantity,
            Brand: new BrandResponse(productEntity.Brand.Id, productEntity.Brand.Name),
            Category: new CategoryResponse(
                productEntity.Category.Id,
                Name: productEntity.Category.Name,
                ParentCategoryId: null,
                Description: null),
            Images: [],
            Attributes: [],
            productEntity.CreatedAt
        );

        ProductRepositoryMock.Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productEntity);
        MapperMock.Setup(x => x.Map<ProductResponse>(productEntity))
            .Returns(responseDto);

        // Act
        var result = await ProductService.GetProductByIdAsync(productId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(responseDto);
    }

    [Fact]
    public async Task Fail_WhenProductDoesNotExist()
    {
        // Arrange
        long productId = 100;
        ProductRepositoryMock.Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await ProductService.GetProductByIdAsync(productId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        result.ErrorCode.Should().Be(Constants.ErrorCode.ProductNotFound);
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrows()
    {
        // Arrange
        long productId = 999;
        ProductRepositoryMock.Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB failure"));

        // Act
        Func<Task> act = async () => await ProductService.GetProductByIdAsync(productId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("DB failure");
    }
}