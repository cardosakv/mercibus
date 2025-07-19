using Auth.Application.DTOs;
using Auth.Domain.Entities;
using Mapster;

namespace Auth.Application.Mappings;

/// <summary>
/// Configuration class for mapping between domain entities and DTOs.
/// </summary>
public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<User, GetUserInfoResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Username, src => src.UserName)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.IsEmailVerified, src => src.EmailConfirmed)
            .Map(dest => dest.Street, src => src.Street)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.State, src => src.State)
            .Map(dest => dest.Country, src => src.Country)
            .Map(dest => dest.PostalCode, src => src.PostalCode)
            .Map(dest => dest.BirthDate, src => src.BirthDate)
            .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl)
            .Map(dest => dest.LastLoginAt, src => src.LastLoginAt)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<UpdateUserInfoRequest, User>
            .NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Street, src => src.Street)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.State, src => src.State)
            .Map(dest => dest.Country, src => src.Country)
            .Map(dest => dest.PostalCode, src => src.PostalCode)
            .Map(dest => dest.BirthDate, src => src.BirthDate)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl);

        TypeAdapterConfig<RegisterRequest, User>
            .NewConfig()
            .Map(dest => dest.UserName, src => src.Username)
            .Map(dest => dest.Email, src => src.Email);
    }
}