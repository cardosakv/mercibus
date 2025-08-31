using Orders.Application.Interfaces.Repositories;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Repositories;

public class OrderRepository(AppDbContext dbContext) : IOrderRepository
{
    public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.Orders.AddAsync(order, cancellationToken);
        return entry.Entity;
    }
}