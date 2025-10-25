using Microsoft.EntityFrameworkCore;
using Payments.Application.Interfaces.Repositories;
using Payments.Domain.Entities;
using Payments.Infrastructure.Configurations;

namespace Payments.Infrastructure;

/// <summary>
/// Application database context.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
    }
}