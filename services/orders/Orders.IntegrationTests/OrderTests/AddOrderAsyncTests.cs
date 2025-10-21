using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Messaging.Events;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Messaging;
using Orders.IntegrationTests.Common;

namespace Orders.IntegrationTests.OrderTests;

public class AddOrderAsyncTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    private const string AddOrderUrl = "api/orders";

    [Fact]
    public async Task ReturnsOk_WhenOrderAddedSuccessfully()
    {
        // Arrange
        var eventPublisher = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IEventPublisher>();
        await eventPublisher.PublishAsync(new ProductAdded(99));
        await Task.Delay(5000);

        var request = new OrderRequest(
        [
            new OrderItemRequest(99, "Phone", 2),
        ]);

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.PostAsJsonAsync(AddOrderUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new OrderRequest(
        [
            new OrderItemRequest(99, "Phone", 2)
        ]);

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.PostAsJsonAsync(AddOrderUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}