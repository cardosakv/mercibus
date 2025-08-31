using MapsterMapper;
using Mercibus.Common.Models;
using Mercibus.Common.Services;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Repositories;
using Orders.Application.Interfaces.Services;
using Orders.Domain.Entities;

namespace Orders.Application.Services;

public class OrderService(IMapper mapper, IAppDbContext dbContext, IOrderRepository orderRepository) : BaseService, IOrderService
{
    public async Task<ServiceResult> AddAsync(OrderRequest request, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Order>(request);
        var order = await orderRepository.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var response = mapper.Map<OrderResponse>(order);
        
        return Success(response);
    }
}