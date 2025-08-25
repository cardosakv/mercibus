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

public class ProductImageService(IProductRepository productRepository, IProductImageRepository productImageRepository, IAppDbContext dbContext, IBlobStorageService blobStorageService, IMapper mapper)
    : BaseService, IProductImageService
{
    public async Task<ServiceResult> AddProductImageAsync(long productId, ProductImageRequest request, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null) return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);

        var fileName = Guid.NewGuid().ToString();
        var fileExtension = Path.GetExtension(request.Image.FileName);
        var blobName = $"{fileName}{fileExtension}";
        await blobStorageService.UploadFileAsync(blobName, request.Image.OpenReadStream());

        var productImage = mapper.Map<ProductImage>(request);
        productImage.ImageUrl = Path.Combine(Constants.BlobStorage.ProductImagesContainer, blobName);
        productImage.ProductId = productId;

        await productImageRepository.AddProductImageAsync(productImage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success();
    }

    public async Task<ServiceResult> DeleteProductImageAsync(long productId, long imageId, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null) return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);

        var productImage = await productImageRepository.GetProductImageByIdAsync(imageId, cancellationToken);
        if (productImage is null) return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ImageNotFound);

        if (productImage.ProductId != productId) return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ImageNotInProduct);

        await blobStorageService.DeleteBlobAsync(productImage.ImageUrl);
        await productImageRepository.DeleteProductImageAsync(productImage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success();
    }
}