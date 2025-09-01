using Mapster;
using Orders.Application.DTOs;
using Orders.Domain.Entities;

namespace Orders.Application.Mappings;

/// <summary>
/// Configures mappings between Order-related DTOs and domain entities.
/// </summary>
public class OrderMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<OrderRequest, Order>
            .NewConfig()
            .Map(dest => dest.Items, src => src.Items);

        TypeAdapterConfig<OrderItemRequest, OrderItem>
            .NewConfig()
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.ProductName, src => src.ProductName)
            .Map(dest => dest.Quantity, src => src.Quantity);

        TypeAdapterConfig<Order, OrderResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.Items, src => src.Items);
        
        TypeAdapterConfig<OrderItem, OrderItemResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.ProductName, src => src.ProductName)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.Price, src => src.Price);
    }
}