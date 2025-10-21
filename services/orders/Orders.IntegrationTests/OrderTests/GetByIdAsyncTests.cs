using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Mercibus.Common.Responses;
using Messaging.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Messaging;
using Orders.Domain.Entities;
using Orders.IntegrationTests.Common;

namespace Orders.IntegrationTests.OrderTests;

public class GetOrderByIdAsyncTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    private const string Url = "api/orders/";

    [Fact]
    public async Task ReturnsOk_WhenOrderExists()
    {
        // Arrange
        var eventPublisher = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IEventPublisher>();
        await eventPublisher.PublishAsync(new ProductAdded(1));
        await Task.Delay(500);

        var db = factory.CreateDbContext();
        var order = await db.Orders.AddAsync(
            new Order
            {
                UserId = "user-1",
                Items =
                [
                    new OrderItem
                    {
                        ProductId = 1,
                        ProductName = "Phone",
                        Price = 1000,
                        Quantity = 1
                    }
                ]
            });
        await db.SaveChangesAsync();

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.GetAsync(Url + order.Entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        var result = JsonConvert.DeserializeObject<OrderResponse>(content!.Data!.ToString()!);
        result!.Id.Should().Be(order.Entity.Id);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenOrderNotFound()
    {
        // Act
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        var response = await client.GetAsync(Url + 9999);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}