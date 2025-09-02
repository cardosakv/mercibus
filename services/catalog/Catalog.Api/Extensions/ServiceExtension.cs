using Catalog.Application.Interfaces.Messaging;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using Catalog.Infrastructure.Messaging;
using Catalog.Infrastructure.Services;

namespace Catalog.Api.Extensions;

/// <summary>
///     Extension method for registering services in the application.
/// </summary>
public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.AddScoped<IProductImageService, ProductImageService>();
        services.AddScoped<IProductReviewService, ProductReviewService>();
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<IEventPublisher, MassTransitEventPublisher>();
    }
}