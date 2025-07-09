using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    public Task<List<Product>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        var products = dbContext.Products.AsQueryable();

        if (query.CategoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == query.CategoryId.Value);
        }

        if (query.BrandId.HasValue)
        {
            products = products.Where(p => p.BrandId == query.BrandId.Value);
        }

        if (query.MinPrice.HasValue)
        {
            products = products.Where(p => p.Price >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= query.MaxPrice.Value);
        }

        if (query.Status.HasValue)
        {
            products = products.Where(p => p.Status == query.Status);
        }

        products = query.SortBy switch
        {
            "price" => query.SortDirection == "asc"
                ? products.OrderBy(p => p.Price)
                : products.OrderByDescending(p => p.Price),

            "name" => query.SortDirection == "asc"
                ? products.OrderBy(p => p.Name)
                : products.OrderByDescending(p => p.Name),

            _ => query.SortDirection == "asc"
                ? products.OrderBy(p => p.CreatedAt)
                : products.OrderByDescending(p => p.CreatedAt)
        };

        return products
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.Products.AddAsync(product, cancellationToken);
        return entry.Entity;
    }

    public Task<Product?> GetProductByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        dbContext.Products.Update(product);
        return Task.CompletedTask;
    }

    public Task DeleteProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        dbContext.Products.Remove(product);
        return Task.CompletedTask;
    }
}