using MapsterMapper;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Mercibus.Common.Services;
using Orders.Application.Common;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Repositories;
using Orders.Application.Interfaces.Services;
using Orders.Domain.Entities;

namespace Orders.Application.Services;

public class OrderService(IMapper mapper, IAppDbContext dbContext, IOrderRepository orderRepository) : BaseService, IOrderService
{
    public async Task<ServiceResult> AddAsync(string? userId, OrderRequest request, CancellationToken cancellationToken = default)
    {
        // if (string.IsNullOrEmpty(userId))
        // {
        //     return Error(ErrorType.AuthenticationError, ErrorCode.Unauthorized);
        // }
        
        var entity = mapper.Map<Order>(request);
        entity.UserId = "userId";
        
        var order = await orderRepository.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var response = mapper.Map<OrderResponse>(order);
        
        return Success(response);
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
        // if (string.IsNullOrEmpty(userId))
        // {
        //     return Error(ErrorType.AuthenticationError, ErrorCode.Unauthorized);
        // }
        
        var orders = await orderRepository.GetByUserIdAsync("userId", cancellationToken);
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

        return Success();
    }
}