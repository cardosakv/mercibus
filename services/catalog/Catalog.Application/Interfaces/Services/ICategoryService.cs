using Catalog.Application.DTOs;
using Mercibus.Common.Models;

namespace Catalog.Application.Interfaces.Services;

/// <summary>
/// Provides methods for retrieving and managing category data.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Asynchronously retrieves a list of categories.
    /// </summary>
    /// <param name="query">The query parameters for retrieving categories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result with a list of <see cref="CategoryResponse"/> objects.</returns>
    Task<ServiceResult> GetCategoriesAsync(CategoryQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new category.
    /// </summary>
    /// <param name="request">The category data to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the add operation.</returns>
    Task<ServiceResult> AddCategoryAsync(CategoryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a category by its unique identifier.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result with a <see cref="CategoryResponse"/> object if found.</returns>
    Task<ServiceResult> GetCategoryByIdAsync(long categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates an existing category.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category to update.</param>
    /// <param name="request">The updated category data.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the update operation.</returns>
    Task<ServiceResult> UpdateCategoryAsync(long categoryId, CategoryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes an existing category.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Result indicating the outcome of the delete operation.</returns>
    Task<ServiceResult> DeleteCategoryAsync(long categoryId, CancellationToken cancellationToken = default);
}