using MapsterMapper;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Mercibus.Common.Services;
using Payments.Application.Common;
using Payments.Application.DTOs;
using Payments.Application.Interfaces.Repositories;
using Payments.Application.Interfaces.Services;

namespace Payments.Application.Services;

public class PaymentService(IPaymentRepository paymentRepository, IMapper mapper) : BaseService, IPaymentService
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
    
    public Task<ServiceResult> InitiatePaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}