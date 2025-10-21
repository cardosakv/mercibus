using FluentAssertions;
using Moq;
using Orders.Application.DTOs;
using Orders.Domain.Entities;

namespace Orders.UnitTests.Application.OrderServiceTests;

public class GetByUserIdAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenOrdersExist()
    {
        // Arrange
        string userId = "user-10";
        var orders = new List<Order>
        {
            new()
            {
                Id = 1,
                UserId = userId
            },
            new()
            {
                Id = 2,
                UserId = userId
            }
        };

        var mapped = new List<OrderResponse>
        {
            new(1, userId, DateTime.UtcNow, "Draft", []),
            new(2, userId, DateTime.UtcNow, "Draft", [])
        };

        OrderRepositoryMock.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(orders);
        MapperMock.Setup(m => m.Map<List<OrderResponse>>(orders))
            .Returns(mapped);

        // Act
        var result = await OrderService.GetByUserIdAsync(userId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        ((List<OrderResponse>)result.Data!).Count.Should().Be(2);
    }

    [Fact]
    public async Task ReturnsError_WhenUserIdIsNull()
    {
        // Act
        var result = await OrderService.GetByUserIdAsync(null, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrows()
    {
        // Arrange
        string userId = "u1";
        OrderRepositoryMock.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB failed"));

        // Act
        Func<Task> act = async () => await OrderService.GetByUserIdAsync(userId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("DB failed");
    }
}