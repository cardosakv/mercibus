using FluentAssertions;
using Moq;
using Orders.Application.DTOs;
using Orders.Domain.Entities;
using Orders.Domain.Enums;

namespace Orders.UnitTests.Application.OrderServiceTests;

public class UpdateAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenOrderIsUpdated()
    {
        // Arrange
        long id = 50;
        var request = new OrderUpdateRequest("Confirmed");
        var order = new Order
        {
            Id = id,
            UserId = "user-50",
            Status = OrderStatus.Draft
        };

        OrderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        MapperMock.Setup(m => m.Map(request, order));
        OrderRepositoryMock.Setup(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await OrderService.UpdateAsync(id, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        OrderRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReturnsError_WhenOrderNotFound()
    {
        // Arrange
        long id = 999;
        var request = new OrderUpdateRequest("Cancelled");

        OrderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await OrderService.UpdateAsync(id, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenRepositoryFails()
    {
        // Arrange
        long id = 10;
        var request = new OrderUpdateRequest("Shipped");
        var order = new Order
        {
            Id = id,
            UserId = "user-1"
        };

        OrderRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        MapperMock.Setup(m => m.Map(request, order));
        OrderRepositoryMock.Setup(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Update failed"));

        // Act
        Func<Task> act = async () => await OrderService.UpdateAsync(id, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Update failed");
    }
}