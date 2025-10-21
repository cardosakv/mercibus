using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces.Repositories;
using Orders.Domain.Entities;
using Orders.Infrastructure.Configurations;

namespace Orders.Infrastructure;

/// <summary>
/// Represents the application's database context.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new OrderConfiguration());
        builder.ApplyConfiguration(new OrderItemConfiguration());
    }
}