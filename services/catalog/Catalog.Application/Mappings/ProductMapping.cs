using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Mapster;

namespace Catalog.Application.Mappings;

/// <summary>
/// Mapping configuration for products.
/// </summary>
public class ProductMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<Product, ProductResponse>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.Sku, src => src.Sku)
            .Map(dest => dest.StockQuantity, src => src.StockQuantity)
            .Map(dest => dest.Brand, src => src.Brand)
            .Map(dest => dest.Category, src => src.Category)
            .Map(dest => dest.Images, src => src.Images)
            .Map(dest => dest.Attributes, src => src.Attributes);

        TypeAdapterConfig<ProductRequest, Product>.NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.Sku, src => src.Sku)
            .Map(dest => dest.StockQuantity, src => src.StockQuantity)
            .Map(dest => dest.BrandId, src => src.BrandId)
            .Map(dest => dest.CategoryId, src => src.CategoryId);
    }
}