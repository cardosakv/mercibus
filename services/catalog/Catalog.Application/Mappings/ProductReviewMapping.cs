using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Mapster;

namespace Catalog.Application.Mappings;

/// <summary>
///     Mapping configuration for product reviews.
/// </summary>
public class ProductReviewMapping
{
    public static void Configure()
    {
        // Entity → Response
        TypeAdapterConfig<ProductReview, ProductReviewResponse>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Rating, src => src.Rating)
            .Map(dest => dest.Comment, src => src.Comment)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        // Request → Entity
        TypeAdapterConfig<ProductReviewRequest, ProductReview>.NewConfig()
            .Map(dest => dest.Rating, src => src.Rating)
            .Map(dest => dest.Comment, src => src.Comment);
    }
}