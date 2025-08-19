using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Mapster;

namespace Catalog.Application.Mappings;

public class ProductImageMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<ProductImage, ProductImageResponse>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl)
            .Map(dest => dest.IsPrimary, src => src.IsPrimary)
            .Map(dest => dest.AltText, src => src.AltText);

        TypeAdapterConfig<ProductImageRequest, ProductImage>.NewConfig()
            .Map(dest => dest.IsPrimary, src => src.IsPrimary)
            .Map(dest => dest.AltText, src => src.AltText);
    }
}