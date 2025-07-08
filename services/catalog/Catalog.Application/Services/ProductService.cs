using AutoMapper;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;

namespace Catalog.Application.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper, IAppDbContext dbContext) : BaseService, IProductService
{
    public async Task<Result> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        var productList = await productRepository.GetProductsAsync(query, cancellationToken);
        var response = mapper.Map<List<ProductResponse>>(productList);

        return Success(data: response);
    }

    public async Task<Result> AddProductAsync(AddProductRequest request, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Product>(request);
        var product = await productRepository.AddProductAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success(resourceId: product.Id);
    }

    public async Task<Result> GetProductByIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.NotFound, Messages.ProductNotFound);
        }
        
        var response = mapper.Map<ProductResponse>(product);
        return Success(data: response);
    }

    public async Task<Result> UpdateProductAsync(long productId, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.NotFound, Messages.ProductNotFound);
        }
        
        mapper.Map(request, product);
        
        await productRepository.UpdateProductAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Success();
    }

    public async Task<Result> DeleteProductAsync(long productId, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.NotFound, Messages.ProductNotFound);
        }
        
        await productRepository.DeleteProductAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Success();
    }
}