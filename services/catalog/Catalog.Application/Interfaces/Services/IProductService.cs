using Catalog.Application.DTOs;

namespace Catalog.Application.Interfaces.Services;

/// <summary>
/// Interface for product service.
/// </summary>
public interface IProductService
{
    Task<List<GetProductResponse>> GetProductsAsync(CancellationToken cancellationToken = default);
}