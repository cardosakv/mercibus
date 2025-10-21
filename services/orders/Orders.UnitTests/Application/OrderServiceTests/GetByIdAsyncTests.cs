using FluentAssertions;
using Moq;
using Orders.Application.DTOs;
using Orders.Domain.Entities;
using Orders.Domain.Enums;

namespace Orders.UnitTests.Application.OrderServiceTests;

public class GetByIdAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenOrderExists()
    {
        // Arrange
        long orderId = 123;
        var order = new Order
        {
            Id = orderId,
            UserId = "user-10",
            Status = OrderStatus.Confirmed,
            Items =
            [
                new OrderItem
                {
                    Id = 1,
                    ProductId = 10,
                    ProductName = "Phone",
                    Price = 1000,
                    Quantity = 1
                }
            ]
        };

        var mapped = new OrderResponse(
            orderId,
            "user-10",
            order.CreatedAt,
            "Confirmed",
            [new(1, 10, "Phone", 1000, 1)]);

        OrderRepositoryMock.Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        MapperMock.Setup(m => m.Map<OrderResponse>(order)).Returns(mapped);

        // Act
        var result = await OrderService.GetByIdAsync(orderId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeOfType<OrderResponse>();
        var data = (OrderResponse)result.Data!;
        data.Id.Should().Be(orderId);
        data.Status.Should().Be("Confirmed");
    }

    [Fact]
    public async Task ReturnsError_WhenOrderNotFound()
    {
        // Arrange
        long id = 99;
        OrderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await OrderService.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrows()
    {
        // Arrange
        long id = 1;
        OrderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB unavailable"));

        // Act
        Func<Task> act = async () => await OrderService.GetByIdAsync(id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("DB unavailable");
    }
}