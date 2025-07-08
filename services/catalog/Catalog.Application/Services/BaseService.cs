using Catalog.Application.Common;

namespace Catalog.Application.Services;

/// <summary>
/// Base service class that provides common methods for handling success and failure responses.
/// </summary>
public abstract class BaseService
{
    /// <summary>
    /// Creates a successful result with optional data and message.
    /// </summary>
    protected static Result Success(object? data = null, string message = "")
    {
        return new Result
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    /// <summary>
    /// Creates a failure result with an error type and optional message.
    /// </summary>
    protected static Result Error(ErrorType errorType = ErrorType.Internal, string message = "")
    {
        return new Result
        {
            IsSuccess = false,
            Message = message,
            ErrorType = errorType
        };
    }
}