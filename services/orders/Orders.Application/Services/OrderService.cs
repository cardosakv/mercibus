using MapsterMapper;
using Mercibus.Common.Models;
using Mercibus.Common.Services;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Repositories;
using Orders.Application.Interfaces.Services;
using Orders.Domain.Entities;

namespace Orders.Application.Services;

public class OrderService(IMapper mapper, IOrderRepository orderRepository) : BaseService, IOrderService
{
    public async Task<ServiceResult> AddAsync(OrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = mapper.Map<Order>(request);
        var entity = await orderRepository.AddAsync(order, cancellationToken);
        var response = mapper.Map<OrderResponse>(entity);
        
        return Success(response);
    }
}