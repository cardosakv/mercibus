namespace Payments.Domain.Enums;

/// <summary>
/// Represents the status of a payment process.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Waiting user to start process.
    /// </summary>
    AwaitingUserAction,
    
    /// <summary>
    /// Currently being processed.
    /// </summary>
    Processing,
    
    /// <summary>
    /// Payment completed successfully.
    /// </summary>
    Completed,
    
    /// <summary>
    /// Payment failed.
    /// </summary>
    Failed
}