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

public class ProductService(IProductRepository productRepository, IBlobStorageService blobStorageService, IMapper mapper, IAppDbContext dbContext)
    : BaseService, IProductService
{
    public async Task<ServiceResult> GetProductsAsync(ProductQuery query, CancellationToken cancellationToken = default)
    {
        var productList = await productRepository.GetProductsAsync(query, cancellationToken);

        foreach (var product in productList)
        {
            foreach (var image in product.Images)
            {
                if (image.ImageUrl.StartsWith(Constants.BlobStorage.ProductImagesContainer))
                {
                    var blobName = image.ImageUrl[(Constants.BlobStorage.ProductImagesContainer.Length + 1)..];
                    var sasTokenExpiryOffset = DateTimeOffset.UtcNow.AddHours(Constants.BlobStorage.BlobTokenExpirationHours);
                    image.ImageUrl = await blobStorageService.GenerateBlobUrlAsync(blobName, sasTokenExpiryOffset);
                }
            }
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
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);
        }

        foreach (var image in product.Images)
        {
            if (image.ImageUrl.StartsWith(Constants.BlobStorage.ProductImagesContainer))
            {
                var blobName = image.ImageUrl[(Constants.BlobStorage.ProductImagesContainer.Length + 1)..];
                var sasTokenExpiryOffset = DateTimeOffset.UtcNow.AddHours(Constants.BlobStorage.BlobTokenExpirationHours);
                image.ImageUrl = await blobStorageService.GenerateBlobUrlAsync(blobName, sasTokenExpiryOffset);
            }
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

    public async Task<ServiceResult> AddProductImageAsync(long productId, ProductImageRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);
        }

        var fileName = Guid.NewGuid().ToString();
        var fileExtension = Path.GetExtension(request.Image.FileName);
        var blobName = $"{fileName}{fileExtension}";
        await blobStorageService.UploadFileAsync(blobName, fileStream: request.Image.OpenReadStream());

        var productImage = mapper.Map<ProductImage>(request);
        productImage.ImageUrl = Path.Combine(Constants.BlobStorage.ProductImagesContainer, blobName);
        productImage.ProductId = productId;

        await productRepository.AddProductImageAsync(productImage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success();
    }

    public async Task<ServiceResult> DeleteProductImageAsync(long productId, long imageId, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);
        }

        var productImage = await productRepository.GetProductImageByIdAsync(imageId, cancellationToken);
        if (productImage is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ImageNotFound);
        }

        if (productImage.ProductId != productId)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ImageNotInProduct);
        }

        await blobStorageService.DeleteBlobAsync(productImage.ImageUrl);
        await productRepository.DeleteProductImageAsync(productImage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success();
    }
}