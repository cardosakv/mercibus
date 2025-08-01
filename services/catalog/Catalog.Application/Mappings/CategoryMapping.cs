using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Mapster;

namespace Catalog.Application.Mappings;

/// <summary>
/// Mapping configuration for categories.
/// </summary>
public class CategoryMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ParentCategoryId, src => src.ParentCategoryId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);

        TypeAdapterConfig<CategoryRequest, Category>.NewConfig()
            .Map(dest => dest.ParentCategoryId, src => src.ParentCategoryId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);
    }
}