using AutoMapper;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Mercibus.Common.Services;

namespace Catalog.Application.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper, IAppDbContext dbContext) : BaseService, IProductService
{
    public async Task<ServiceResult> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        var productList = await productRepository.GetProductsAsync(query, cancellationToken);
        var response = mapper.Map<List<ProductResponse>>(productList);

        return Success(response);
    }

    public async Task<ServiceResult> AddProductAsync(AddProductRequest request, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Product>(request);
        var product = await productRepository.AddProductAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<ProductResponse>(product);

        return Success(response);
    }

    public async Task<ServiceResult> GetProductByIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);
        }

        var response = mapper.Map<ProductResponse>(product);

        return Success(response);
    }

    public async Task<ServiceResult> UpdateProductAsync(long productId, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);
        }

        mapper.Map(request, product);

        await productRepository.UpdateProductAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success();
    }

    public async Task<ServiceResult> DeleteProductAsync(long productId, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);
        }

        await productRepository.DeleteProductAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success();
    }
}