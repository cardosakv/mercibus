using Auth.Application.Common;
using Auth.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace Auth.Api.Filters;

/// <summary>
/// Custom validation result factory.
/// </summary>
public class ValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(ActionExecutingContext context,
        ValidationProblemDetails? validationProblemDetails)
    {
        var badRequestParams = new List<BadRequestParams>();

        if (validationProblemDetails is not null && validationProblemDetails.Errors.Any())
        {
            badRequestParams.AddRange(validationProblemDetails.Errors
                .Select(error => new BadRequestParams
                {
                    Field = error.Key,
                    Code = error.Value[0]
                }));
        }

        return new BadRequestObjectResult(
            new ApiErrorResponse
            {
                Error = new ApiError
                {
                    Type = ErrorType.InvalidRequestError,
                    Code = ErrorCode.ValidationFailed,
                    Params = badRequestParams
                }
            }
        );
    }
}