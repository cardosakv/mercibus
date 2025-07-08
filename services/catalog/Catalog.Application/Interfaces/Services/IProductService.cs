using Catalog.Application.Common;
using Catalog.Application.DTOs;

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
    Task<Result> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new product.
    /// </summary>
    /// <param name="request">The product data to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the add operation.</returns>
    Task<Result> AddProductAsync(AddProductRequest request, CancellationToken cancellationToken = default);
}