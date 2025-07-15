using Common.Models;

namespace Auth.Application.Services;

/// <summary>
/// Base service class providing common functionality for services.
/// </summary>
public abstract class BaseService
{
    /// <summary>
    /// Creates a successful result with optional data.
    /// </summary>
    protected static ServiceResult Success(object? data = null)
    {
        return new ServiceResult
        {
            IsSuccess = true,
            Data = data
        };
    }

    /// <summary>
    ///  Creates an error result with specified error type, code, and optional message.
    /// </summary>
    protected static ServiceResult Error(string type, string code, string? message = null)
    {
        return new ServiceResult
        {
            IsSuccess = false,
            ErrorType = type,
            ErrorCode = code,
            Message = message
        };
    }
}