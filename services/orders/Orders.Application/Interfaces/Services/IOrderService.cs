using Mercibus.Common.Models;
using Orders.Application.DTOs;

namespace Orders.Application.Interfaces.Services;

public interface IOrderService
{
    /// <summary>
    /// Adds a new order.
    /// </summary>
    /// <param name="userId">ID of the user placing the order.</param>
    /// <param name="request">Order request containing order details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Service result indicating success or failure.</returns>
    Task<ServiceResult> AddAsync(string? userId, OrderRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Service result containing the order if found; otherwise, an error result.</returns>
    Task<ServiceResult> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves all orders for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose orders to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Service result containing a list of orders for the user.</returns>
    Task<ServiceResult> GetByUserIdAsync(string? userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="id">The unique identifier of the order to update.</param>
    /// <param name="request">Order update request containing updated order details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Service result indicating success or failure of the update operation.</returns>
    Task<ServiceResult> UpdateAsync(long id, OrderUpdateRequest request, CancellationToken cancellationToken = default);
}