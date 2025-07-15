using System.Net;
using Auth.Application.Common;
using Auth.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

/// <summary>
/// Base controller for handling API responses.
/// </summary>
[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Ok(ServiceResult result)
    {
        if (!result.IsSuccess)
        {
            return Error(result);
        }

        return Ok(new ApiSuccessResponse { Data = result.Data });
    }

    private ObjectResult Error(ServiceResult result)
    {
        return result.ErrorType switch
        {
            ErrorType.InvalidRequestError =>
                StatusCode((int)HttpStatusCode.BadRequest, new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Type = result.ErrorType ?? ErrorType.InvalidRequestError,
                        Code = result.ErrorCode ?? ErrorCode.ValidationFailed
                    }
                }),

            ErrorType.ConflictError =>
                StatusCode((int)HttpStatusCode.Conflict, new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Type = result.ErrorType ?? ErrorType.ConflictError,
                        Code = result.ErrorCode ?? ErrorCode.Internal
                    }
                }),

            ErrorType.AuthenticationError =>
                StatusCode((int)HttpStatusCode.Unauthorized, new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Type = result.ErrorType ?? ErrorType.AuthenticationError,
                        Code = result.ErrorCode ?? ErrorCode.Internal
                    }
                }),

            ErrorType.PermissionError =>
                StatusCode((int)HttpStatusCode.Forbidden, new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Type = result.ErrorType ?? ErrorType.PermissionError,
                        Code = result.ErrorCode ?? ErrorCode.Internal
                    }
                }),

            ErrorType.LockedError =>
                StatusCode((int)HttpStatusCode.Locked, new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Type = result.ErrorType ?? ErrorType.LockedError,
                        Code = result.ErrorCode ?? ErrorCode.Internal
                    }
                }),

            _ =>
                StatusCode((int)HttpStatusCode.InternalServerError, new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Type = result.ErrorType ?? ErrorType.ApiError,
                        Code = result.ErrorCode ?? ErrorCode.Internal
                    }
                })
        };
    }
}