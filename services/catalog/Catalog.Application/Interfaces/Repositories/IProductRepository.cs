using Catalog.Application.DTOs;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories;

/// <summary>
/// Repository interface for accessing and managing <see cref="Product"/> entities.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves a list of products based on the specified query parameters.
    /// </summary>
    /// <param name="query">The query parameters for filtering products.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of matching <see cref="Product"/> entities.</returns>
    Task<List<Product>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default);
}