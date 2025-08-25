using Catalog.Application.DTOs;
using Mercibus.Common.Models;

namespace Catalog.Application.Interfaces.Services;

/// <summary>
///     Provides methods for retrieving and managing product image data.
/// </summary>
public interface IProductImageService
{
    /// <summary>
    ///     Asynchronously adds an image to a product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to which the image will be added.</param>
    /// <param name="request">The request containing the image data and metadata.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the add image operation.</returns>
    Task<ServiceResult> AddProductImageAsync(long productId, ProductImageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously deletes an image from a product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="productImageId">The unique identifier of the product image.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the delete image operation.</returns>
    Task<ServiceResult> DeleteProductImageAsync(long productId, long productImageId, CancellationToken cancellationToken = default);
}