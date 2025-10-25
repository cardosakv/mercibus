using System.Net;
using FluentAssertions;
using Messaging.Events;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Interfaces.Messaging;
using Orders.Domain.Entities;
using Orders.IntegrationTests.Common;

namespace Orders.IntegrationTests.OrderTests;

public class GetOrdersByUserIdAsyncTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    private const string Url = "api/orders";

    [Fact]
    public async Task ReturnsOk_WhenOrdersExistForUser()
    {
        // Arrange
        var eventPublisher = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IEventPublisher>();
        await eventPublisher.PublishAsync(new ProductAdded(1));
        await Task.Delay(500);

        var db = factory.CreateDbContext();
        await db.Orders.AddAsync(
            new Order
            {
                UserId = "user-1",
                Currency = "PHP",
            });
        await db.Orders.AddAsync(
            new Order
            {
                UserId = "user-1",
                Currency = "PHP",
            });
        await db.SaveChangesAsync();

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.GetAsync(Url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ReturnsOk_WhenNoOrdersExist()
    {
        // Arrange
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.GetAsync(Url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}