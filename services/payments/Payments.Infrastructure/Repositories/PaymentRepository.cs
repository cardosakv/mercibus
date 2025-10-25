using Microsoft.EntityFrameworkCore;
using Payments.Application.Interfaces.Repositories;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Repositories;

public class PaymentRepository(AppDbContext dbContext) : IPaymentRepository
{
    public async Task<Payment?> GetPaymentByIdAsync(long paymentId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Payments.FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken);
    }

    public Task<Payment?> GetPaymentByOrderIdAsync(long orderId, CancellationToken cancellationToken = default)
    {
        return dbContext.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId, cancellationToken);
    }

    public async Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    { 
        await dbContext.Payments.AddAsync(payment, cancellationToken);
    }

    public Task UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        dbContext.Payments.Update(payment);
        return Task.CompletedTask;
    }
}