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
    Task<List<Product>> GetProductsAsync(ProductQuery query, CancellationToken cancellationToken = default);

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

    /// <summary>
    /// Updates an existing <see cref="Product"/> entity in the repository.
    /// </summary>
    /// <param name="product">The <see cref="Product"/> entity to update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Reduces the stock quantity of a product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="quantity">The quantity to reduce from the product's stock.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>>True if the stock was successfully reduced; otherwise, false.</returns>
    Task<bool> ReduceProductStockAsync(long productId, int quantity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="Product"/> entity from the repository.
    /// </summary>
    /// <param name="product">The <see cref="Product"/> entity to delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteProductAsync(Product product, CancellationToken cancellationToken = default);
}