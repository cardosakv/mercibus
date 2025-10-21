using MapsterMapper;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Mercibus.Common.Services;
using Messaging.Events;
using Orders.Application.Common;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Messaging;
using Orders.Application.Interfaces.Repositories;
using Orders.Application.Interfaces.Services;
using Orders.Domain.Entities;
using EventOrderItem = Messaging.Models.OrderItem;

namespace Orders.Application.Services;

public class OrderService(IMapper mapper, IAppDbContext dbContext, IOrderRepository orderRepository, IEventPublisher eventPublisher) : BaseService, IOrderService
{
    public async Task<ServiceResult> AddAsync(string? userId, OrderRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Error(ErrorType.AuthenticationError, ErrorCode.Unauthorized);
        }

        var order = mapper.Map<Order>(request);
        order.UserId = userId;

        await orderRepository.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await eventPublisher.PublishAsync(
            new OrderCreated(
                OrderId: order.Id,
                CustomerId: order.UserId,
                CreatedAt: order.CreatedAt,
                Items: order.Items.Select(item => new EventOrderItem(item.ProductId, item.Quantity)).ToList()
            ),
            cancellationToken);

        return Success();
    }

    public async Task<ServiceResult> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.OrderNotFound);
        }

        var response = mapper.Map<OrderResponse>(order);

        return Success(response);
    }

    public async Task<ServiceResult> GetByUserIdAsync(string? userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Error(ErrorType.AuthenticationError, ErrorCode.Unauthorized);
        }

        var orders = await orderRepository.GetByUserIdAsync(userId, cancellationToken);
        var response = mapper.Map<List<OrderResponse>>(orders);

        return Success(response);
    }

    public async Task<ServiceResult> UpdateAsync(long id, OrderUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.OrderNotFound);
        }

        mapper.Map(request, order);

        await orderRepository.UpdateAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success();
    }
}