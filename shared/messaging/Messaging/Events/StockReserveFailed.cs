namespace Messaging.Events;

/// <summary>
/// Event representing a failed stock reservation.
/// </summary>
public record StockReserveFailed(
    long OrderId,
    List<long> UnavailableProductIds
);