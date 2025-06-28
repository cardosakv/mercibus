using Auth.Application.DTOs;
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
        return new BadRequestObjectResult(
            new BadRequestResponse
            {
                Errors = validationProblemDetails?.Errors.SelectMany(x => x.Value).ToList() ?? []
            }
        );
    }
}