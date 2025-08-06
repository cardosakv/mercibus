using Catalog.Application.Mappings;
using Mapster;

namespace Catalog.Api.Extensions;

/// <summary>
/// Extension methods for configuring mapping in the application.
/// </summary>
public static class MappingExtension
{
    public static void AddMapping(this IServiceCollection services)
    {
        services.AddMapster();
        ProductMapping.Configure();
    }
}