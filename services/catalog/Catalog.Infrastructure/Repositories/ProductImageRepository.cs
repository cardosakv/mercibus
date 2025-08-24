using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

public class ProductImageRepository(AppDbContext dbContext) : IProductImageRepository
{
    public Task<ProductImage?> GetProductImageByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return dbContext.ProductImages
            .AsNoTracking()
            .FirstOrDefaultAsync(pi => pi.Id == id, cancellationToken);
    }

    public Task<ProductImage> AddProductImageAsync(ProductImage productImage, CancellationToken cancellationToken = default)
    {
        var entry = dbContext.ProductImages.Add(productImage);
        return Task.FromResult(entry.Entity);
    }

    public Task DeleteProductImageAsync(ProductImage productImage, CancellationToken cancellationToken = default)
    {
        dbContext.ProductImages.Remove(productImage);
        return Task.CompletedTask;
    }
}