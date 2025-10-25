using Mapster;
using Payments.Application.DTOs;
using Payments.Domain.Entities;

namespace Payments.Application.Mappings;

public class PaymentMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<Payment, PaymentResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.OrderId, src => src.OrderId)
            .Map(dest => dest.CustomerId, src => src.CustomerId)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Currency, src => src.Currency)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
    }
}