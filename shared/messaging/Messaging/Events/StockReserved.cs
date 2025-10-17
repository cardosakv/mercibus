namespace Messaging.Events;

/// <summary>
/// Event representing that stock has been reserved for an order.
/// </summary>
public record StockReserved(
    long OrderId
);