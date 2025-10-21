using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Messaging.Events;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Messaging;
using Orders.Domain.Entities;
using Orders.Domain.Enums;
using Orders.IntegrationTests.Common;

namespace Orders.IntegrationTests.OrderTests;

public class UpdateOrderAsyncTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    private const string Url = "api/orders/";

    [Fact]
    public async Task ReturnsOk_WhenOrderUpdatedSuccessfully()
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
                Status = OrderStatus.Draft,
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

        await eventPublisher.PublishAsync(new StockReserveFailed(1, "user-1", [1]));
        await Task.Delay(500);
        await eventPublisher.PublishAsync(new StockReserved(1, "user-1"));
        await Task.Delay(500);

        var update = new OrderUpdateRequest("Confirmed");

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.PatchAsJsonAsync(Url + order.Entity.Id, update);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await factory.CreateDbContext().Orders.FindAsync(order.Entity.Id);
        updated!.Status.Should().Be(OrderStatus.Confirmed);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenOrderDoesNotExist()
    {
        // Arrange
        var update = new OrderUpdateRequest("Cancelled");
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.PatchAsJsonAsync(Url + 9999, update);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}