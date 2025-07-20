namespace Mercibus.Common.Constants;

/// <summary>
/// Error types for API responses.
/// </summary>
public static class ErrorType
{
    public const string InvalidRequestError = "invalid_request_error";
    public const string ConflictError = "conflict_error";
    public const string AuthenticationError = "authentication_error";
    public const string LockedError = "locked_error";
    public const string PermissionError = "permission_error";
    public const string ApiError = "api_error";
}