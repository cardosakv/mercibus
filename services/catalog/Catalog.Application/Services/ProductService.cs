using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using MapsterMapper;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Mercibus.Common.Services;

namespace Catalog.Application.Services;

public class ProductService(IProductRepository productRepository, IBlobStorageService blobStorageService, IMapper mapper, IAppDbContext dbContext, ICacheService cacheService)
    : BaseService, IProductService
{
    public async Task<ServiceResult> GetProductsAsync(ProductQuery query, CancellationToken cancellationToken = default)
    {
        var productList = await productRepository.GetProductsAsync(query, cancellationToken);

        foreach (var image in productList.SelectMany(product => product.Images))
        {
            await ReplaceImageUrlWithToken(image);
        }

        var response = mapper.Map<List<ProductResponse>>(productList);

        return Success(response);
    }

    public async Task<ServiceResult> AddProductAsync(ProductRequest request, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Product>(request);
        var product = await productRepository.AddProductAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<ProductResponse>(product);

        return Success(response);
    }

    public async Task<ServiceResult> GetProductByIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        var product = await cacheService.GetAsync<Product>(Constants.Redis.ProductPrefix + productId);
        if (product is null)
        {
            product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
            if (product is null)
            {
                return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);
            }

            await cacheService.SetAsync(key: Constants.Redis.ProductPrefix + productId, product, Constants.Redis.CacheExpiration);
        }

        foreach (var image in product.Images)
        {
            await ReplaceImageUrlWithToken(image);
        }

        var response = mapper.Map<ProductResponse>(product);

        return Success(response);
    }

    public async Task<ServiceResult> UpdateProductAsync(long productId, ProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);
        }

        mapper.Map(request, product);

        await productRepository.UpdateProductAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await cacheService.RemoveAsync(Constants.Redis.ProductPrefix + productId);

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
        await cacheService.RemoveAsync(Constants.Redis.ProductPrefix + productId);

        return Success();
    }

    private async Task ReplaceImageUrlWithToken(ProductImage image)
    {
        if (image.ImageUrl.StartsWith(Constants.BlobStorage.ProductImagesContainer))
        {
            var blobName = image.ImageUrl[(Constants.BlobStorage.ProductImagesContainer.Length + 1)..];
            var sasTokenExpiryOffset = DateTimeOffset.UtcNow.AddHours(Constants.BlobStorage.BlobTokenExpirationHours);
            image.ImageUrl = await blobStorageService.GenerateBlobUrlAsync(blobName, sasTokenExpiryOffset);
        }
    }
}