using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

public class BrandRepository(AppDbContext dbContext) : IBrandRepository
{
    public Task<List<Brand>> GetBrandsAsync(BrandQuery query, CancellationToken cancellationToken = default)
    {
        var brands = dbContext.Brands.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Region))
        {
            brands = brands.Where(b => b.Region == query.Region);
        }

        return brands
            .OrderBy(b => b.Name)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Brand> AddBrandAsync(Brand brand, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.Brands.AddAsync(brand, cancellationToken);
        return entry.Entity;
    }

    public Task<Brand?> GetBrandByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return dbContext.Brands
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public Task UpdateBrandAsync(Brand brand, CancellationToken cancellationToken = default)
    {
        dbContext.Brands.Update(brand);
        return Task.CompletedTask;
    }

    public Task DeleteBrandAsync(Brand brand, CancellationToken cancellationToken = default)
    {
        dbContext.Brands.Remove(brand);
        return Task.CompletedTask;
    }

    public Task<bool> DoesBrandExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return dbContext.Brands
            .AsNoTracking()
            .AnyAsync(b => b.Id == id, cancellationToken);
    }

    public Task<bool> IsBrandUsedInProductsAsync(long id, CancellationToken cancellationToken = default)
    {
        return dbContext.Products
            .AsNoTracking()
            .AnyAsync(p => p.BrandId == id, cancellationToken);
    }
}