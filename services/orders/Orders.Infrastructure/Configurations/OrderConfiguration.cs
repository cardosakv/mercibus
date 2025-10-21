using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}