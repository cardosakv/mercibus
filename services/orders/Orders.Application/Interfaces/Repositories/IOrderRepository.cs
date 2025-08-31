using Orders.Domain.Entities;

namespace Orders.Application.Interfaces.Repositories;

/// <summary>
/// Repository interface for managing orders.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Adds a new order.
    /// </summary>
    /// <param name="order">Order to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The added order.</returns>
    Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);
}