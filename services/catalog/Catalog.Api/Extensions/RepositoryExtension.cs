using Catalog.Application.Interfaces.Repositories;
using Catalog.Infrastructure.Repositories;

namespace Catalog.Api.Extensions;

/// <summary>
///     Extension method for registering repositories in the application.
/// </summary>
public static class RepositoryExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
    }
}