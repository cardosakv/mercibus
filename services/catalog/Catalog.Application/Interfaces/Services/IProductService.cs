using Catalog.Application.DTOs;
using Mercibus.Common.Models;

namespace Catalog.Application.Interfaces.Services;

/// <summary>
/// Provides methods for retrieving and managing product data.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Asynchronously retrieves a list of products.
    /// </summary>
    /// <param name="query">The query parameters for retrieving products.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result with a list of <see cref="ProductResponse"/> objects.</returns>
    Task<ServiceResult> GetProductsAsync(ProductQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new product.
    /// </summary>
    /// <param name="request">The product data to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the add operation.</returns>
    Task<ServiceResult> AddProductAsync(ProductRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result with a <see cref="ProductResponse"/> object if found.</returns>
    Task<ServiceResult> GetProductByIdAsync(long productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates an existing product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to update.</param>
    /// <param name="request">The updated product data.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the update operation.</returns>
    Task<ServiceResult> UpdateProductAsync(long productId, ProductRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes an existing product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the delete operation.</returns>
    Task<ServiceResult> DeleteProductAsync(long productId, CancellationToken cancellationToken = default);
}