using Catalog.Application.Interfaces.Messaging;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Messaging.Events;
using Messaging.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.IntegrationTests.Messaging;

public class OrderCreatedConsumerTests(MessageWebAppFactory factory) : IClassFixture<MessageWebAppFactory>
{
    [Fact]
    public async Task ConsumesOrderCreatedEvent_WhenPublished()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var eventPublisher = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IEventPublisher>();

        var testCategory = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Category 1"
            });
        var testBrand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "Brand 1"
            });
        await dbContext.SaveChangesAsync();

        var product = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Test Product",
                Description = "This is a test product",
                Sku = "TEST-SKU",
                Price = 100,
                StockQuantity = 100,
                CategoryId = testCategory.Entity.Id,
                BrandId = testBrand.Entity.Id
            });
        await dbContext.SaveChangesAsync();

        // Act
        await eventPublisher.PublishAsync(
            new OrderCreated(
                OrderId: 1,
                CustomerId: "user-1",
                Items:
                [
                    new OrderItem(product.Entity.Id, Quantity: 1)
                ],
                DateTime.UtcNow
            )
        );
        await Task.Delay(1000);

        // Assert
        dbContext = factory.CreateDbContext();
        var updatedProduct = await dbContext.Products.FindAsync(product.Entity.Id);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.StockQuantity.Should().Be(99);
    }

    [Fact]
    public async Task PublishesStockReserveFailed_WhenStockIsInsufficient()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var eventPublisher = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IEventPublisher>();

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Cat"
            });
        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "Brand"
            });
        await dbContext.SaveChangesAsync();

        var product = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Low Stock Product",
                Sku = "LS-01",
                Price = 50,
                StockQuantity = 1,
                CategoryId = category.Entity.Id,
                BrandId = brand.Entity.Id
            });
        await dbContext.SaveChangesAsync();

        // Act
        await eventPublisher.PublishAsync(
            new OrderCreated(
                OrderId: 100,
                CustomerId: "user-2",
                Items: [new OrderItem(product.Entity.Id, Quantity: 5)],
                DateTime.UtcNow
            ));
        await Task.Delay(1000); // allow consumer to process

        // Assert
        dbContext = factory.CreateDbContext();
        var updated = await dbContext.Products.FindAsync(product.Entity.Id);
        updated!.StockQuantity.Should().Be(1);
    }
}