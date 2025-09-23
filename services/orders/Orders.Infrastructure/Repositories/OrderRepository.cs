using Microsoft.EntityFrameworkCore;
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

    public async Task<Order?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders.AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<List<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders.AsNoTracking()
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .ToListAsync(cancellationToken);
    }
}