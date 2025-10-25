using FluentAssertions;
using Messaging.Events;
using Moq;
using Orders.Application.DTOs;
using Orders.Domain.Entities;
using OrderItem = Orders.Domain.Entities.OrderItem;

namespace Orders.UnitTests.Application.OrderServiceTests;

public class AddAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenOrderIsAdded()
    {
        // Arrange
        const string userId = "user-1";
        var request = new OrderRequest(
            "PHP",
            [
                new(101, "Laptop", 2),
                new(202, "Mouse", 1)
            ]);

        var order = new Order
        {
            Id = 10,
            UserId = userId,
            Currency = "PHP",
            Items = new List<OrderItem>
            {
                new()
                {
                    ProductId = 101,
                    ProductName = "Laptop",
                    Price = 500,
                    Quantity = 2
                },
                new()
                {
                    ProductId = 202,
                    ProductName = "Mouse",
                    Price = 20,
                    Quantity = 1
                }
            }
        };

        MapperMock.Setup(m => m.Map<Order>(request)).Returns(order);
        OrderRepositoryMock.Setup(r => r.AddAsync(order, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        EventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<OrderCreated>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await OrderService.AddAsync(userId, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        OrderRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        EventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<OrderCreated>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReturnsError_WhenUserIdIsNull()
    {
        // Arrange
        var request = new OrderRequest("PHP", []);

        // Act
        var result = await OrderService.AddAsync(null, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenRepositoryFails()
    {
        // Arrange
        const string userId = "user-1";
        var request = new OrderRequest("PHP", []);
        var order = new Order
        {
            UserId = userId,
            Currency = "PHP",
        };

        MapperMock.Setup(m => m.Map<Order>(request)).Returns(order);
        OrderRepositoryMock.Setup(r => r.AddAsync(order, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB insert failed"));

        // Act
        Func<Task> act = async () => await OrderService.AddAsync(userId, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("DB insert failed");
    }
}