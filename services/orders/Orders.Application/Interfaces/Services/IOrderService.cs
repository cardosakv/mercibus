using Mercibus.Common.Models;
using Orders.Domain.Entities;

namespace Orders.Application.Interfaces.Services;

public interface IOrderService
{
    /// <summary>
    /// Adds a new order.
    /// </summary>
    /// <param name="order">Order to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Service result indicating success or failure.</returns>
    Task<ServiceResult> AddAsync(Order order, CancellationToken cancellationToken = default);
}