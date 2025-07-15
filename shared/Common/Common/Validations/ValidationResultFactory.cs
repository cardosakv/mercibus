using Common.Constants;
using Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using System.Text.Json;

namespace Common.Validations;

/// <summary>
/// Custom validation result factory.
/// </summary>
public class ValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
    {
        if (validationProblemDetails is not null && validationProblemDetails.Errors.Any())
        {
            if (validationProblemDetails.Errors.Any(error => string.Equals(error.Key, "request", StringComparison.OrdinalIgnoreCase)))
            {
                return new BadRequestObjectResult(new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Type = ErrorType.InvalidRequestError,
                        Code = ErrorCode.RequestBodyEmpty
                    }
                });
            }

            var badRequestParams = validationProblemDetails.Errors
                .Select(error => new BadRequestParams
                {
                    Field = JsonNamingPolicy.CamelCase.ConvertName(error.Key),
                    Code = error.Value.LastOrDefault() ?? ErrorCode.ValidationFailed,
                });

            return new BadRequestObjectResult(new ApiErrorResponse
            {
                Error = new ApiError
                {
                    Type = ErrorType.InvalidRequestError,
                    Code = ErrorCode.ValidationFailed,
                    Params = badRequestParams.ToList()
                }
            });
        }

        return new BadRequestObjectResult(new ApiErrorResponse
        {
            Error = new ApiError
            {
                Type = ErrorType.InvalidRequestError,
                Code = ErrorCode.Internal
            }
        });
    }
}