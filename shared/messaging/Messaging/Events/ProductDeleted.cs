namespace Messaging.Events;

/// <summary>
/// Event raised when a product is deleted from the catalog.
/// </summary>
public record ProductDeleted(
    long ProductId
);