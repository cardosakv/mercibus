using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Mapster;

namespace Catalog.Application.Mappings;

/// <summary>
/// Mapping configuration for brands.
/// </summary>
public class BrandMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<Brand, BrandResponse>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.LogoUrl, src => src.LogoUrl)
            .Map(dest => dest.Region, src => src.Region)
            .Map(dest => dest.Website, src => src.Website)
            .Map(dest => dest.AdditionalInfo, src => src.AdditionalInfo);

        TypeAdapterConfig<BrandRequest, Brand>.NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.LogoUrl, src => src.LogoUrl)
            .Map(dest => dest.Region, src => src.Region)
            .Map(dest => dest.Website, src => src.Website)
            .Map(dest => dest.AdditionalInfo, src => src.AdditionalInfo);
    }
}