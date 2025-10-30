using Catalog.Application.Interfaces.Messaging;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Messaging.Events;
using Messaging.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.IntegrationTests.Messaging;

public class OrderFailedConsumerTests(MessageWebAppFactory factory) : IClassFixture<MessageWebAppFactory>
{
    [Fact]
    public async Task ConsumesOrderFailedEvent_WhenPublished()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var eventPublisher = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IEventPublisher>();

        // Create required category and brand
        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Electronics"
            });
        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "Brand A"
            });
        await dbContext.SaveChangesAsync();

        // Create a product that initially had its stock reduced by an order
        var product = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Laptop",
                Sku = "LAP-001",
                Description = "Gaming laptop",
                Price = 1200,
                StockQuantity = 5, // current stock before restoring
                CategoryId = category.Entity.Id,
                BrandId = brand.Entity.Id
            });
        await dbContext.SaveChangesAsync();

        // Act
        await eventPublisher.PublishAsync(
            new OrderFailed(
                OrderId: 1,
                CustomerId: "user-1",
                Items:
                [
                    new OrderItem(product.Entity.Id, Quantity: 2)
                ],
                DateTime.UtcNow
            )
        );
        await Task.Delay(5000);

        // Assert
        dbContext = factory.CreateDbContext();
        var updatedProduct = await dbContext.Products.FindAsync(product.Entity.Id);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.StockQuantity.Should().Be(7); // 5 + 2 restored
    }

    [Fact]
    public async Task DoesNothing_WhenProductDoesNotExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var eventPublisher = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IEventPublisher>();

        // Ensure database is empty
        await dbContext.Products.ExecuteDeleteAsync();

        // Act
        await eventPublisher.PublishAsync(
            new OrderFailed(
                OrderId: 99,
                CustomerId: "user-x",
                Items:
                [
                    new OrderItem(ProductId: 999, Quantity: 3)
                ],
                DateTime.UtcNow
            )
        );
        await Task.Delay(1000); // Allow consumer to process

        // Assert
        dbContext = factory.CreateDbContext();
        var products = await dbContext.Products.ToListAsync();
        products.Should().BeEmpty();
    }
}