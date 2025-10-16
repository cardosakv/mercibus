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

    /// <summary>
    /// Retrieves all orders for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of orders for the user.</returns>
    Task<List<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="order">Order to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
}