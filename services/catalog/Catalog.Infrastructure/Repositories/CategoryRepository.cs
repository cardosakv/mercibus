using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
{
    public Task<List<Category>> GetCategoriesAsync(CategoryQuery query, CancellationToken cancellationToken = default)
    {
        var categories = dbContext.Categories.AsQueryable();

        if (query.ParentCategoryId.HasValue)
        {
            categories = categories.Where(c => c.ParentCategoryId == query.ParentCategoryId.Value);
        }

        return categories
            .OrderBy(c => c.Name)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.Categories.AddAsync(category, cancellationToken);
        return entry.Entity;
    }

    public Task<Category?> GetCategoryByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        dbContext.Categories.Update(category);
        return Task.CompletedTask;
    }

    public Task DeleteCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        dbContext.Categories.Remove(category);
        return Task.CompletedTask;
    }

    public Task<bool> DoesCategoryExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return dbContext.Categories
            .AsNoTracking()
            .AnyAsync(c => c.Id == id, cancellationToken);
    }

    public Task<bool> IsCategoryUsedInProductsAsync(long id, CancellationToken cancellationToken = default)
    {
        return dbContext.Products
            .AsNoTracking()
            .AnyAsync(p => p.CategoryId == id, cancellationToken);
    }
}