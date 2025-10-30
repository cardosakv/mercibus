using Payments.Application.DTOs;

namespace Payments.Application.Interfaces.Services;

/// <summary>
/// Gateway client to process payments.
/// </summary>
public interface IPaymentClient
{
    /// <summary>
    /// Initiates a payment process.
    /// </summary>
    /// <param name="request">The payment details to initiate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>Redirect URL for checkout.</returns>
    Task<string> Initiate(PaymentClientRequest request, CancellationToken cancellationToken = default);
}