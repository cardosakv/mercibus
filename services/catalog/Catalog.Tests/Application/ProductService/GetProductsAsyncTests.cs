using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using FluentAssertions;
using Moq;

namespace Catalog.Tests.Application.ProductService;

public class GetProductsAsyncTests : BaseTest
{
    private readonly GetProductsQuery _query = new(
        CategoryId: null,
        BrandId: null,
        MinPrice: null,
        MaxPrice: null,
        Status: null
    );

    [Fact]
    public async Task Success_WhenProductsExist()
    {
        // Arrange
        var productEntities = new List<Product>()
        {
            new()
            {
                Id = 1,
                Name = "Product 1",
                Description = "Description 1",
                Sku = "SKU1",
                Price = 10.0m,
                Status = ProductStatus.Listed,
                StockQuantity = 100,
                CategoryId = 1,
                BrandId = 1,
                CreatedAt = DateTime.UtcNow,
            },
            new()
            {
                Id = 2,
                Name = "Product 2",
                Description = "Description 2",
                Sku = "SKU2",
                Price = 10.0m,
                Status = ProductStatus.Listed,
                StockQuantity = 100,
                CategoryId = 1,
                BrandId = 1,
                CreatedAt = DateTime.UtcNow,
            }
        };

        var productResponseList = new List<ProductResponse>()
        {
            new(
                Id: 1,
                Name: "Product 1",
                Description: "Description 1",
                Price: 10.0m,
                Sku: "SKU1",
                Status: ProductStatus.Listed,
                StockQuantity: 100,
                Brand: new BrandResponse(1, "Brand 1"),
                Category: new CategoryResponse(1, "Category 1", null),
                Images: [],
                Attributes: [],
                CreatedAt: DateTime.UtcNow
            ),
            new(
                Id: 1,
                Name: "Product 2",
                Description: "Description 2",
                Price: 10.0m,
                Sku: "SKU2",
                Status: ProductStatus.Listed,
                StockQuantity: 100,
                Brand: new BrandResponse(1, "Brand 1"),
                Category: new CategoryResponse(1, "Category 1", null),
                Images: [],
                Attributes: [],
                CreatedAt: DateTime.UtcNow
            ),
        };

        ProductRepositoryMock.Setup(x => x.GetProductsAsync(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(productEntities);
        MapperMock.Setup(x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
            .Returns(productResponseList);

        // Act
        var result = await ProductService.GetProductsAsync(_query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(productResponseList);
    }

    [Fact]
    public async Task Success_WhenNoProductsExist()
    {
        // Arrange
        var emptyProductList = new List<Product>();
        var emptyResponseList = new List<ProductResponse>();

        ProductRepositoryMock.Setup(x => x.GetProductsAsync(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyProductList);
        MapperMock.Setup(x => x.Map<List<ProductResponse>>(emptyProductList))
            .Returns(emptyResponseList);

        // Act
        var result = await ProductService.GetProductsAsync(_query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(emptyResponseList);
    }

    [Fact]
    public async Task Success_WhenFilteredByStatus()
    {
        // Arrange
        var filteredQuery = _query with { Status = ProductStatus.Listed };

        var productList = new List<Product>
        {
            new()
            {
                Id = 3,
                Name = "Filtered Product",
                Description = "Filtered Desc",
                Sku = "F-SKU",
                Price = 20,
                Status = ProductStatus.Listed,
                StockQuantity = 50,
                CategoryId = 1,
                BrandId = 1,
                CreatedAt = DateTime.UtcNow,
            }
        };

        var responseList = new List<ProductResponse>
        {
            new(
                Id: 3,
                Name: "Filtered Product",
                Description: "Filtered Desc",
                Price: 20,
                Sku: "F-SKU",
                Status: ProductStatus.Listed,
                StockQuantity: 50,
                Brand: new BrandResponse(1, "Brand 1"),
                Category: new CategoryResponse(1, "Category 1", null),
                Images: [],
                Attributes: [],
                CreatedAt: DateTime.UtcNow
            )
        };

        ProductRepositoryMock.Setup(x => x.GetProductsAsync(filteredQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productList);
        MapperMock.Setup(x => x.Map<List<ProductResponse>>(productList))
            .Returns(responseList);

        // Act
        var result = await ProductService.GetProductsAsync(filteredQuery, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(responseList);
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrowsException()
    {
        // Arrange
        ProductRepositoryMock.Setup(x => x.GetProductsAsync(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Something went wrong"));

        // Act
        Func<Task> act = async () => await ProductService.GetProductsAsync(_query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Something went wrong");
    }
}