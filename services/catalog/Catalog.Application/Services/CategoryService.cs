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

public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IAppDbContext dbContext) : BaseService, ICategoryService
{
    public async Task<ServiceResult> GetCategoriesAsync(CategoryQuery query, CancellationToken cancellationToken = default)
    {
        var categoryList = await categoryRepository.GetCategoriesAsync(query, cancellationToken);
        var response = mapper.Map<List<CategoryResponse>>(categoryList);

        return Success(response);
    }

    public async Task<ServiceResult> AddCategoryAsync(CategoryRequest request, CancellationToken cancellationToken = default)
    {
        if (request.ParentCategoryId is not null)
        {
            var parentCategoryExists = await categoryRepository.DoesCategoryExistsAsync(request.ParentCategoryId.Value, cancellationToken);
            if (!parentCategoryExists)
            {
                return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ParentCategoryNotFound);
            }
        }

        var entity = mapper.Map<Category>(request);
        var category = await categoryRepository.AddCategoryAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<CategoryResponse>(category);
        return Success(response);
    }

    public async Task<ServiceResult> GetCategoryByIdAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetCategoryByIdAsync(categoryId, cancellationToken);
        if (category is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.CategoryNotFound);
        }

        var response = mapper.Map<CategoryResponse>(category);
        return Success(response);
    }

    public async Task<ServiceResult> UpdateCategoryAsync(long categoryId, CategoryRequest request, CancellationToken cancellationToken = default)
    {
        if (request.ParentCategoryId is not null)
        {
            var parentCategoryExists = await categoryRepository.DoesCategoryExistsAsync(request.ParentCategoryId.Value, cancellationToken);
            if (!parentCategoryExists)
            {
                return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ParentCategoryNotFound);
            }
        }

        var category = await categoryRepository.GetCategoryByIdAsync(categoryId, cancellationToken);
        if (category is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.CategoryNotFound);
        }

        mapper.Map(request, category);
        await categoryRepository.UpdateCategoryAsync(category, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success();
    }

    public async Task<ServiceResult> DeleteCategoryAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetCategoryByIdAsync(categoryId, cancellationToken);
        if (category is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.CategoryNotFound);
        }

        var isUsed = await categoryRepository.DoesCategoryUsedInProductsAsync(categoryId, cancellationToken);
        if (isUsed)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.CategoryInUse);
        }

        await categoryRepository.DeleteCategoryAsync(category, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success();
    }
}