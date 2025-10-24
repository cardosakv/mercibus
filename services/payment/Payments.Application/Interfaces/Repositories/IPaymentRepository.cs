using Payments.Domain.Entities;

namespace Payments.Application.Interfaces.Repositories;

/// <summary>
/// Repository interface for managing payment data.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Retrieves a payment by its unique identifier.
    /// </summary>
    /// <param name="paymentId">The unique identifier of the payment.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The payment details if found; otherwise, null.</returns>
    Task<Payment?> GetPaymentByIdAsync(long paymentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new payment record.
    /// </summary>
    /// <param name="payment">The payment details to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created payment record.</returns>
    Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing payment record.
    /// </summary>
    /// <param name="payment">The payment details to update.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The updated payment record.</returns>
    Task UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken = default);
}