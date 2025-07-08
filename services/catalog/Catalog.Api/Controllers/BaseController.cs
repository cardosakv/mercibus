using System.Net;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleGet(Result result)
    {
        return result.IsSuccess ? Ok(result.Data) : HandleErrorResponse(result);
    }

    protected IActionResult HandlePost(Result result)
    {
        if (result.IsSuccess)
        {
            return result.Data is not null
                ? Ok(result.Data)
                : Ok(new StandardResponse { Message = result.Message });
        }

        return HandleErrorResponse(result);
    }

    protected IActionResult HandlePutOrDelete(Result result)
    {
        return result.IsSuccess ? NoContent() : HandleErrorResponse(result);
    }

    private IActionResult HandleErrorResponse(Result result)
    {
        return result.ErrorType switch
        {
            ErrorType.BadRequest or ErrorType.Validation =>
                BadRequest(new BadRequestResponse { Errors = [result.Message] }),
            ErrorType.NotFound =>
                NotFound(new StandardResponse { Message = result.Message }),
            ErrorType.Conflict =>
                Conflict(new StandardResponse { Message = result.Message }),
            ErrorType.Unauthorized =>
                Unauthorized(new StandardResponse { Message = result.Message }),
            ErrorType.Forbidden =>
                Forbid(),
            ErrorType.Locked =>
                StatusCode((int)HttpStatusCode.Locked, new StandardResponse { Message = result.Message }),
            _ =>
                StatusCode((int)HttpStatusCode.InternalServerError, new StandardResponse { Message = result.Message })
        };
    }
}