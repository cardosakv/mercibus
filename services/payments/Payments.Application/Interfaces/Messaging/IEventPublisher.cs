namespace Payments.Application.Interfaces.Messaging;

/// <summary>
/// Interface for publishing events to a message broker or event bus.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an event asynchronously to the message broker or event bus.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    /// <param name="eventMessage">Event to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default) where TEvent : class;
}