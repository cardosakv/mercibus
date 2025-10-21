namespace Messaging.Events;

/// <summary>
/// Event raised when a new product is added to the catalog.
/// </summary>
public record ProductAdded(
    long ProductId
);