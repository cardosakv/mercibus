using Catalog.Application.DTOs;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories;

/// <summary>
/// Repository interface for accessing and managing <see cref="Category"/> entities.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Retrieves a list of categories based on the specified query parameters.
    /// </summary>
    /// <param name="query">The query parameters for filtering categories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of matching <see cref="Category"/> entities.</returns>
    Task<List<Category>> GetCategoriesAsync(CategoryQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new <see cref="Category"/> entity to the repository.
    /// </summary>
    /// <param name="category">The <see cref="Category"/> entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The created <see cref="Category"/> entity.</returns>
    Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a <see cref="Category"/> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The matching <see cref="Category"/> entity, or null if not found.</returns>
    Task<Category?> GetCategoryByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing <see cref="Category"/> entity in the repository.
    /// </summary>
    /// <param name="category">The <see cref="Category"/> entity to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="Category"/> entity from the repository.
    /// </summary>
    /// <param name="category">The <see cref="Category"/> entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteCategoryAsync(Category category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a category with the specified unique identifier exists in the repository.
    /// </summary>
    /// <param name="id">Category ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Boolean indicating whether the category exists or not.</returns>
    Task<bool> DoesCategoryExistsAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a category with the specified unique identifier is used in any products.
    /// </summary>
    /// <param name="id">Category ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Boolean indication whether the category is used or not.</returns>
    Task<bool> DoesCategoryUsedInProductsAsync(long id, CancellationToken cancellationToken = default);
}