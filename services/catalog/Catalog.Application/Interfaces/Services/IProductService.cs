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
    /// <returns>A list of <see cref="ProductResponse"/> objects.</returns>
    Task<List<ProductResponse>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default);
}