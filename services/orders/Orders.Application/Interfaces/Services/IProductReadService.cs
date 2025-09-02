namespace Orders.Application.Interfaces.Services;

/// <summary>
/// Service for reading product information.
/// </summary>
public interface IProductReadService
{
    /// <summary>
    /// Checks if a product exists by its ID.
    /// </summary>
    /// <param name="productId">The ID of the product to check.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if the product exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(long productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a product by its ID.
    /// </summary>
    /// <param name="productId">The ID of the product to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if the product was added successfully; otherwise, false.</returns>
    Task<bool> AddAsync(long productId, CancellationToken cancellationToken = default);
}