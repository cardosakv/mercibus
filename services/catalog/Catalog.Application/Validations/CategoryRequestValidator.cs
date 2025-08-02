using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Repositories;
using FluentValidation;

namespace Catalog.Application.Validations;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.NameRequired)
            .MaximumLength(Constants.CategoryValidation.MaxNameLength).WithMessage(Constants.ErrorCode.NameTooLong);

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(Constants.CategoryValidation.MaxDescriptionLength).WithMessage(Constants.ErrorCode.DescriptionTooLong);

        RuleFor(x => x.ParentCategoryId)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (id, cancellationToken) =>
            {
                if (id is null)
                {
                    return true;
                }

                return await categoryRepository.DoesCategoryExistsAsync(id.Value, cancellationToken);
            })
            .WithMessage(Constants.ErrorCode.ParentCategoryNotFound);
    }
}