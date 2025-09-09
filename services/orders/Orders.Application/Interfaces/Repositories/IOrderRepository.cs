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
    
    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The order if found; otherwise, <c>null</c>.</returns>
    Task<Order?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}