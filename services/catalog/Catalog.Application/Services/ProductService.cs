using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;

namespace Catalog.Application.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
    public async Task<List<ProductResponse>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var entityList = await productRepository.GetProductsAsync(query, cancellationToken);
            var response = mapper.Map<List<ProductResponse>>(entityList);
            return response;
        }
        catch (Exception e)
        {
            return [];
        }
    }
}