using Mercibus.Common.Models;
using Orders.Application.DTOs;

namespace Orders.Application.Interfaces.Services;

public interface IOrderService
{
    /// <summary>
    /// Adds a new order.
    /// </summary>
    /// <param name="request">Order request containing order details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Service result indicating success or failure.</returns>
    Task<ServiceResult> AddAsync(OrderRequest request, CancellationToken cancellationToken = default);
}