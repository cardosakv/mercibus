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
    
    /// <summary>
    /// Adds a new <see cref="Product"/> entity to the repository.
    /// </summary>
    /// <param name="product">The <see cref="Product"/> entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The created <see cref="Product"/> entity.</returns>
    Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a <see cref="Product"/> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The matching <see cref="Product"/> entity, or null if not found.</returns>
    Task<Product?> GetProductByIdAsync(long id, CancellationToken cancellationToken = default);
}