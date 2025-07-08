using AutoMapper;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;

namespace Catalog.Application.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper) : BaseService, IProductService
{
    public async Task<Result> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var entityList = await productRepository.GetProductsAsync(query, cancellationToken);
            var response = mapper.Map<List<ProductResponse>>(entityList);

            return Success(response);
        }
        catch (Exception)
        {
            return Error(ErrorType.Internal, Messages.UnexpectedError);
        }
    }
}