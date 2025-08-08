using Catalog.Application.DTOs;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories;

/// <summary>
/// Repository interface for accessing and managing <see cref="Brand"/> entities.
/// </summary>
public interface IBrandRepository
{
    /// <summary>
    /// Retrieves a list of brands based on the specified query parameters.
    /// </summary>
    /// <param name="query">The query parameters for filtering brands.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of matching <see cref="Brand"/> entities.</returns>
    Task<List<Brand>> GetBrandsAsync(BrandQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new <see cref="Brand"/> entity to the repository.
    /// </summary>
    /// <param name="brand">The <see cref="Brand"/> entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The created <see cref="Brand"/> entity.</returns>
    Task<Brand> AddBrandAsync(Brand brand, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a <see cref="Brand"/> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the brand.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The matching <see cref="Brand"/> entity, or null if not found.</returns>
    Task<Brand?> GetBrandByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing <see cref="Brand"/> entity in the repository.
    /// </summary>
    /// <param name="brand">The <see cref="Brand"/> entity to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateBrandAsync(Brand brand, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="Brand"/> entity from the repository.
    /// </summary>
    /// <param name="brand">The <see cref="Brand"/> entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteBrandAsync(Brand brand, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a brand with the specified unique identifier exists in the repository.
    /// </summary>
    /// <param name="id">Brand ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Boolean indicating whether the brand exists or not.</returns>
    Task<bool> DoesBrandExistsAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a brand with the specified unique identifier is used in any products.
    /// </summary>
    /// <param name="id">Brand ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Boolean indication whether the brand is used or not.</returns>
    Task<bool> IsBrandUsedInProductsAsync(long id, CancellationToken cancellationToken = default);
}