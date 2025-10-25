using MapsterMapper;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Mercibus.Common.Services;
using Payments.Application.Common;
using Payments.Application.DTOs;
using Payments.Application.Interfaces.Repositories;
using Payments.Application.Interfaces.Services;
using Payments.Domain.Enums;

namespace Payments.Application.Services;

public class PaymentService(IPaymentClient paymentClient, IPaymentRepository paymentRepository, IAppDbContext dbContext, IMapper mapper) : BaseService, IPaymentService
{
    public async Task<ServiceResult> GetPaymentByIdAsync(long paymentId, CancellationToken cancellationToken = default)
    {
        var payment = await paymentRepository.GetPaymentByIdAsync(paymentId, cancellationToken);
        if (payment is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.PaymentNotFound);
        }

        var response = mapper.Map<PaymentResponse>(payment);

        return Success(response);
    }

    public async Task<ServiceResult> InitiatePaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var payment = await paymentRepository.GetPaymentByOrderIdAsync(request.OrderId, cancellationToken);
        if (payment is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.PaymentNotFound);
        }

        switch (payment.Status)
        {
            case PaymentStatus.Processing:
                return Error(ErrorType.ConflictError, Constants.ErrorCode.PaymentCurrentlyProcessing);
            case PaymentStatus.Completed:
                return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.PaymentAlreadyCompleted);
            case PaymentStatus.Failed:
                return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.PaymentFailed);
        }

        payment.Status = PaymentStatus.Processing;
        payment.UpdatedAt = DateTime.UtcNow;
        await paymentRepository.UpdatePaymentAsync(payment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var paymentClientRequest = new PaymentClientRequest(
            ReferenceId: payment.Id.ToString(),
            Amount: payment.Amount,
            Currency: payment.Currency,
            Country: request.BillingRequest.Country,
            SessionType: Constants.PaymentClient.SessionType,
            Mode: Constants.PaymentClient.Mode
        );

        try
        {
            var paymentLinkUrl = await paymentClient.Initiate(paymentClientRequest, cancellationToken);
            return Success(paymentLinkUrl);
        }
        catch (Exception)
        {
            payment.Status = PaymentStatus.Failed;
            payment.UpdatedAt = DateTime.UtcNow;
            await paymentRepository.UpdatePaymentAsync(payment, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ServiceResult> ProcessPaymentWebhookAsync(PaymentWebhookRequest request, CancellationToken cancellationToken = default)
    {
        if (!long.TryParse(request.Data.ReferenceId, out var referenceId))
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.PaymentNotFound);
        }
        
        var payment = await paymentRepository.GetPaymentByIdAsync(referenceId, cancellationToken);
        if (payment is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.PaymentNotFound);
        }
        
        payment.Status = request.Data.Status == Constants.PaymentClient.SuccessStatus ? PaymentStatus.Completed : PaymentStatus.Failed;
        payment.UpdatedAt = DateTime.UtcNow;
        await paymentRepository.UpdatePaymentAsync(payment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Success();
    }
}