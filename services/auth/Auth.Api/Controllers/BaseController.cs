using System.Net;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

/// <summary>
/// Base controller for handling API responses.
/// </summary>
[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResponse(Response response, string httpMethod)
    {
        if (response.IsSuccess)
        {
            return httpMethod switch
            {
                "GET" => Ok(response.Data),
                "POST" => response.Data is not null
                    ? Ok(response.Data)
                    : Ok(new StandardResponse { Message = response.Message }),
                "PUT" or "DELETE" => NoContent(),
                _ => Ok(response.Data)
            };
        }

        return HandleErrorResponse(response);
    }

    private IActionResult HandleErrorResponse(Response response)
    {
        return response.ErrorType switch
        {
            ErrorType.BadRequest => BadRequest(new BadRequestResponse { Errors = [response.Message] }),
            ErrorType.Validation => BadRequest(new BadRequestResponse { Errors = [response.Message] }),
            ErrorType.NotFound => NotFound(new StandardResponse { Message = response.Message }),
            ErrorType.Conflict => Conflict(new StandardResponse { Message = response.Message }),
            ErrorType.Unauthorized => Unauthorized(new StandardResponse { Message = response.Message }),
            ErrorType.Forbidden => Forbid(),
            ErrorType.Locked => StatusCode((int)HttpStatusCode.Locked,
                new StandardResponse { Message = response.Message }),
            _ => StatusCode((int)HttpStatusCode.InternalServerError,
                new StandardResponse { Message = response.Message })
        };
    }
}