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

public class BrandService(IBrandRepository brandRepository, IMapper mapper, IAppDbContext dbContext, ICacheService cacheService) : BaseService, IBrandService
{
    public async Task<ServiceResult> GetBrandsAsync(BrandQuery query, CancellationToken cancellationToken = default)
    {
        var brandList = await brandRepository.GetBrandsAsync(query, cancellationToken);
        var response = mapper.Map<List<BrandResponse>>(brandList);

        return Success(response);
    }

    public async Task<ServiceResult> AddBrandAsync(BrandRequest request, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Brand>(request);
        var brand = await brandRepository.AddBrandAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<BrandResponse>(brand);
        return Success(response);
    }

    public async Task<ServiceResult> GetBrandByIdAsync(long brandId, CancellationToken cancellationToken = default)
    {
        var brand = await cacheService.GetAsync<Brand>(Constants.Redis.BrandPrefix + brandId);
        if (brand is null)
        {
            brand = await brandRepository.GetBrandByIdAsync(brandId, cancellationToken);
            if (brand is null)
            {
                return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.BrandNotFound);
            }

            await cacheService.SetAsync(key: Constants.Redis.BrandPrefix + brandId, brand, Constants.Redis.CacheExpiration);
        }

        var response = mapper.Map<BrandResponse>(brand);
        return Success(response);
    }

    public async Task<ServiceResult> UpdateBrandAsync(long brandId, BrandRequest request, CancellationToken cancellationToken = default)
    {
        var brand = await brandRepository.GetBrandByIdAsync(brandId, cancellationToken);
        if (brand is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.BrandNotFound);
        }

        mapper.Map(request, brand);
        await brandRepository.UpdateBrandAsync(brand, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await cacheService.RemoveAsync(Constants.Redis.BrandPrefix + brandId);

        return Success();
    }

    public async Task<ServiceResult> DeleteBrandAsync(long brandId, CancellationToken cancellationToken = default)
    {
        var brand = await brandRepository.GetBrandByIdAsync(brandId, cancellationToken);
        if (brand is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.BrandNotFound);
        }

        var isUsed = await brandRepository.IsBrandUsedInProductsAsync(brandId, cancellationToken);
        if (isUsed)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.BrandInUse);
        }

        await brandRepository.DeleteBrandAsync(brand, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await cacheService.RemoveAsync(Constants.Redis.BrandPrefix + brandId);

        return Success();
    }
}