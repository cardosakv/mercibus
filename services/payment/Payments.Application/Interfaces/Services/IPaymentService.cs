using Mercibus.Common.Models;
using Payments.Application.DTOs;
using Payments.Domain.Entities;

namespace Payments.Application.Interfaces.Services
{
    /// <summary>
    /// Service interface for managing payment operations.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Retrieves a payment by its unique identifier.
        /// </summary>
        /// <param name="paymentId">The unique identifier of the payment.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>Result containing the payment details if found; otherwise, error details.</returns>
        Task<ServiceResult> GetPaymentByIdAsync(long paymentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Initiates a payment process.
        /// </summary>
        /// <param name="request">The payment details to initiate.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>Result containing the redirect URL for checkout or error details.</returns>
        Task<ServiceResult> InitiatePaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default);
    }
}