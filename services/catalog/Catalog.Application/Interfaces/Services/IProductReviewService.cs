using Catalog.Application.DTOs;
using Mercibus.Common.Models;

namespace Catalog.Application.Interfaces.Services;

/// <summary>
///     Service interface for managing product reviews.
/// </summary>
public interface IProductReviewService
{
    /// <summary>
    ///     Retrieves a list of product reviews based on the specified query parameters.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="query">The query parameters for filtering product reviews.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result with a list of matching <see cref="ProductReviewResponse" /> records.</returns>
    Task<ServiceResult> GetProductReviewsAsync(long productId, ProductReviewQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a product review by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="reviewId">The unique identifier of the product review.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result with the matching <see cref="ProductReviewResponse" />, or an error if not found.</returns>
    Task<ServiceResult> GetProductReviewByIdAsync(long productId, long reviewId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a new product review.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="userId">The unique identifier of the user who submitted the review.</param>
    /// <param name="request">The request containing review details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result with the created <see cref="ProductReviewResponse" />.</returns>
    Task<ServiceResult> AddProductReviewAsync(long productId, string? userId, ProductReviewRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an existing product review.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="reviewId">The unique identifier of the product review.</param>
    /// <param name="request">The request containing updated review details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result with the updated <see cref="ProductReviewResponse" />.</returns>
    Task<ServiceResult> UpdateProductReviewAsync(long productId, long reviewId, ProductReviewRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a product review by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="reviewId">The unique identifier of the product review.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the delete operation.</returns>
    Task<ServiceResult> DeleteProductReviewAsync(long productId, long reviewId, CancellationToken cancellationToken = default);
}