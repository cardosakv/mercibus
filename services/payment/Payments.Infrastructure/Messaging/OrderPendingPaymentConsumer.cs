using MassTransit;
using Messaging.Events;
using Payments.Application.Interfaces.Repositories;
using Payments.Domain.Entities;
using Payments.Domain.Enums;

namespace Payments.Infrastructure.Messaging;

public class OrderPendingPaymentConsumer(IPaymentRepository paymentRepository, IAppDbContext dbContext): IConsumer<OrderPendingPayment>
{
    public async Task Consume(ConsumeContext<OrderPendingPayment> context)
    {
        var message = context.Message;

        var payment = await paymentRepository.GetPaymentByIdAsync(message.OrderId);
        if (payment is null)
        {
            var newPayment = new Payment()
            {
                OrderId = message.OrderId,
                CustomerId = message.CustomerId,
                Amount = message.TotalAmount,
                Currency = message.Currency,
                Status = PaymentStatus.AwaitingUserAction,
                CreatedAt = DateTime.UtcNow
            };
            
            await paymentRepository.AddPaymentAsync(newPayment, context.CancellationToken);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}