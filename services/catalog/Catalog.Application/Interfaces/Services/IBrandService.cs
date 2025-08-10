using Catalog.Application.DTOs;
using Mercibus.Common.Models;

namespace Catalog.Application.Interfaces.Services;

/// <summary>
/// Provides methods for retrieving and managing brand data.
/// </summary>
public interface IBrandService
{
    /// <summary>
    /// Asynchronously retrieves a list of brands.
    /// </summary>
    /// <param name="query">The query parameters for retrieving brands.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result with a list of <see cref="BrandResponse"/> objects.</returns>
    Task<ServiceResult> GetBrandsAsync(BrandQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new brand.
    /// </summary>
    /// <param name="request">The brand data to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the add operation.</returns>
    Task<ServiceResult> AddBrandAsync(BrandRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a brand by its unique identifier.
    /// </summary>
    /// <param name="brandId">The unique identifier of the brand.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result with a <see cref="BrandResponse"/> object if found.</returns>
    Task<ServiceResult> GetBrandByIdAsync(long brandId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates an existing brand.
    /// </summary>
    /// <param name="brandId">The unique identifier of the brand to update.</param>
    /// <param name="request">The updated brand data.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the update operation.</returns>
    Task<ServiceResult> UpdateBrandAsync(long brandId, BrandRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes an existing brand.
    /// </summary>
    /// <param name="brandId">The unique identifier of the brand to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the delete operation.</returns>
    Task<ServiceResult> DeleteBrandAsync(long brandId, CancellationToken cancellationToken = default);
}