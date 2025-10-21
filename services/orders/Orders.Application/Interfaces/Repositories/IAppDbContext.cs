namespace Orders.Application.Interfaces.Repositories;

/// <summary>
/// Interface for the application database context.
/// </summary>
public interface IAppDbContext
{
    /// <summary>
    /// Gets the database context for the application.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}