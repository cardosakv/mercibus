using AuthService.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using System.Net;

namespace AuthService.Api.Controllers
{
    /// <summary>
    /// Base controller for handling API responses.
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Handles the API response based on the provided ApiResponse object.
        /// </summary>
        /// <typeparam name="T">API response type.</typeparam>
        /// <param name="response">Api response.</param>
        /// <returns>Modified action result.</returns>
        protected IActionResult HandleResponse<T>(Result<T> response, string httpMethod)
        {
            if (response.IsSuccess)
            {
                var method = new HttpMethod(httpMethod);

                if (method == HttpMethod.Get)
                {
                    return Ok(response.Data);
                }

                if (method == HttpMethod.Post)
                {
                    return Created(string.Empty, response.Data);
                }

                if (method == HttpMethod.Put || method == HttpMethod.Delete)
                {
                    return NoContent();
                }

                return Ok(response.Data);
            }

            // Handle error responses based on the ErrorType if the operation was not successful
            return response.ErrorType switch
            {
                ErrorType.NotFound => NotFound(new { response.Message }),
                ErrorType.Conflict => Conflict(new { response.Message }),
                ErrorType.Unauthorized => Unauthorized(new { response.Message }),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, new { response.Message })
            };
        }
    }

    /// <summary>
    /// Custom result factory for FluentValidation auto-validation.
    /// </summary>
    public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            return new BadRequestObjectResult(
                new
                {
                    Message = "One or more validations errors occured.",
                    validationProblemDetails?.Errors
                }
                );
        }
    }
}
