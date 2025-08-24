using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories;

/// <summary>
///  Repository interface for accessing and managing <see cref="ProductImage" /> entities.
/// </summary>
public interface IProductImageRepository
{
    /// <summary>
    /// Retrieves a <see cref="ProductImage" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product image.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The matching <see cref="ProductImage" /> entity, or null if not found.</returns>
    Task<ProductImage?> GetProductImageByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new <see cref="ProductImage" /> entity to the repository.
    /// </summary>
    /// <param name="productImage">The <see cref="ProductImage" /> entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The created <see cref="ProductImage" /> entity.</returns>
    Task<ProductImage> AddProductImageAsync(ProductImage productImage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="ProductImage" /> entity from the repository.
    /// </summary>
    /// <param name="productImage">The <see cref="ProductImage" /> entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteProductImageAsync(ProductImage productImage, CancellationToken cancellationToken = default);
}