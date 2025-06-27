using Auth.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auth.Api.Controllers
{
    /// <summary>
    /// Base controller for handling API responses.
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleResponse<T>(Response<T> response, string httpMethod)
        {
            if (response.IsSuccess)
            {
                return httpMethod switch
                {
                    "GET" => Ok(response.Data),
                    "POST" => Created(string.Empty, new { response.Message }),
                    "PUT" or "DELETE" => NoContent(),
                    _ => Ok(response.Data)
                };
            }

            return HandleErrorResponse(response);
        }

        protected IActionResult HandleResponse(Response response, string httpMethod)
        {
            if (response.IsSuccess)
            {
                return httpMethod switch
                {
                    "POST" => Created(string.Empty, new { response.Message }),
                    "PUT" or "DELETE" => NoContent(),
                    _ => Ok(new { response.Message })
                };
            }

            return HandleErrorResponse(response);
        }

        private IActionResult HandleErrorResponse(Response response)
        {
            return response.ErrorType switch
            {
                ErrorType.BadRequest => BadRequest(new { response.Message }),
                ErrorType.Validation => BadRequest(new { response.Message }),
                ErrorType.NotFound => NotFound(new { response.Message }),
                ErrorType.Conflict => Conflict(new { response.Message }),
                ErrorType.Unauthorized => Unauthorized(new { response.Message }),
                ErrorType.Forbidden => Forbid(),
                ErrorType.Locked => StatusCode((int)HttpStatusCode.Locked, new { response.Message }),
                ErrorType.Internal => StatusCode((int)HttpStatusCode.InternalServerError, new { response.Message }),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, new { response.Message })
            };
        }
    }
}
