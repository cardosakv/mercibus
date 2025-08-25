using Catalog.Application.Common;
using Catalog.Application.DTOs;
using FluentValidation;

namespace Catalog.Application.Validations;

public class ProductReviewRequestValidator : AbstractValidator<ProductReviewRequest>
{
    public ProductReviewRequestValidator()
    {
        RuleFor(x => x.Rating)
            .Cascade(CascadeMode.Stop)
            .InclusiveBetween(from: 1, to: 5).WithMessage(Constants.ErrorCode.InvalidRating);

        RuleFor(x => x.Comment)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(Constants.ProductValidation.ReviewMaxCommentLength)
            .WithMessage(Constants.ErrorCode.CommentTooLong);
    }
}