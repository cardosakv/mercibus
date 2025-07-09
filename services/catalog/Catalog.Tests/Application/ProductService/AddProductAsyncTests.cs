using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using FluentAssertions;
using Moq;

namespace Catalog.Tests.Application.ProductService;

public class AddProductAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenProductIsAdded()
    {
        // Arrange
        var request = new AddProductRequest(
            Name: "Test Product",
            Description: "Test Description",
            Sku: "TP-001",
            Price: 199.99m,
            StockQuantity: 10,
            Status: "listed",
            CategoryId: 1,
            BrandId: 2
        );

        var mappedEntity = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Sku = request.Sku,
            Price = request.Price,
            Status = ProductStatus.Listed,
            StockQuantity = request.StockQuantity,
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,
            Category = new Category { Id = 1, Name = "Category 1" },
            Brand = new Brand { Id = 2, Name = "Brand 2" },
            CreatedAt = DateTime.UtcNow
        };

        var savedEntity = mappedEntity;
        savedEntity.Id = 99;

        MapperMock.Setup(m => m.Map<Product>(request)).Returns(mappedEntity);
        ProductRepositoryMock
            .Setup(r => r.AddProductAsync(mappedEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedEntity);
        DbContextMock.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await ProductService.AddProductAsync(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResourceId.Should().Be(savedEntity.Id);
        result.Message.Should().Be(Messages.ProductAdded);
    }
    
    [Fact]
    public async Task Success_WhenSaveChangesReturnsZero()
    {
        // Arrange
        var request = new AddProductRequest(
            Name: "No Save Product",
            Description: "Test Desc",
            Sku: "NOSAVE-01",
            Price: 50,
            StockQuantity: 3,
            Status: "listed",
            CategoryId: 1,
            BrandId: 1
        );

        var entity = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Sku = request.Sku,
            Price = request.Price,
            Status = ProductStatus.Listed,
            StockQuantity = request.StockQuantity,
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,
            Category = new Category { Id = 1, Name = "Category 1" },
            Brand = new Brand { Id = 1, Name = "Brand 1" },
            CreatedAt = DateTime.UtcNow
        };

        var saved = entity;
        saved.Id = 55;

        MapperMock.Setup(m => m.Map<Product>(request)).Returns(entity);
        ProductRepositoryMock.Setup(r => r.AddProductAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(saved);
        DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await ProductService.AddProductAsync(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResourceId.Should().Be(55);
    }
    
    [Fact]
    public async Task Throws_WhenAddFails()
    {
        // Arrange
        var request = new AddProductRequest(
            Name: "Broken Product",
            Description: "Fails",
            Sku: "ERR-001",
            Price: 10,
            StockQuantity: 1,
            Status: "listed",
            CategoryId: 1,
            BrandId: 2
        );

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Sku = request.Sku,
            Price = request.Price,
            Status = ProductStatus.Listed,
            StockQuantity = request.StockQuantity,
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,
            Category = new Category { Id = 1, Name = "Category 1" },
            Brand = new Brand { Id = 2, Name = "Brand 2" },
            CreatedAt = DateTime.UtcNow
        };

        MapperMock.Setup(m => m.Map<Product>(request)).Returns(product);
        ProductRepositoryMock
            .Setup(r => r.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Insert failed"));

        // Act
        Func<Task> act = async () => await ProductService.AddProductAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Insert failed");
    }
}