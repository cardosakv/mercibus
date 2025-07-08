using AutoMapper;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;

namespace Catalog.Application.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper, IAppDbContext dbContext) : BaseService, IProductService
{
    public async Task<Result> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        var entityList = await productRepository.GetProductsAsync(query, cancellationToken);
        var response = mapper.Map<List<ProductResponse>>(entityList);

        return Success(response);
    }

    public async Task<Result> AddProductAsync(AddProductRequest request, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Domain.Entities.Product>(request); 
        await productRepository.AddProductAsync(entity, cancellationToken);
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Error(ErrorType.Internal, Messages.UnexpectedError);
        }

        return Success();
    }
}