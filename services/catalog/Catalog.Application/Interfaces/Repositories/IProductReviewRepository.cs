using Catalog.Application.DTOs;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories;

/// <summary>
///     Repository interface for accessing and managing <see cref="ProductReview" /> entities.
/// </summary>
public interface IProductReviewRepository
{
    /// <summary>
    ///     Retrieves a list of product reviews based on the specified product id and query parameters.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="query">The query parameters for filtering product reviews.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of matching <see cref="ProductReview" /> entities.</returns>
    Task<List<ProductReview>> GetProductReviewsAsync(long productId, ProductReviewQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a new <see cref="ProductReview" /> entity to the repository.
    /// </summary>
    /// <param name="review">The <see cref="ProductReview" /> entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The created <see cref="ProductReview" /> entity.</returns>
    Task<ProductReview> AddProductReviewAsync(ProductReview review, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a <see cref="ProductReview" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product review.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The matching <see cref="ProductReview" /> entity, or null if not found.</returns>
    Task<ProductReview?> GetProductReviewByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an existing <see cref="ProductReview" /> entity in the repository.
    /// </summary>
    /// <param name="review">The <see cref="ProductReview" /> entity to update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateProductReviewAsync(ProductReview review, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a <see cref="ProductReview" /> entity from the repository.
    /// </summary>
    /// <param name="review">The <see cref="ProductReview" /> entity to delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteProductReviewAsync(ProductReview review, CancellationToken cancellationToken = default);
}