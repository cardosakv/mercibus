namespace Auth.Application.Common;

/// <summary>
/// Error types for service responses.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Request is invalid or malformed.
    /// </summary>
    BadRequest,

    /// <summary>
    /// Validation errors occurred.
    /// </summary>
    Validation,

    /// <summary>
    /// Resource is not found.
    /// </summary>
    NotFound,

    /// <summary>
    /// Resource already exists or conflict occurs.
    /// </summary>
    Conflict,

    /// <summary>
    /// Unauthorized access or authentication failure.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// Resource access is forbidden.
    /// </summary>
    Forbidden,

    /// <summary>
    /// Resource is locked or temporarily unavailable.
    /// </summary>
    Locked,

    /// <summary>
    /// Internal server error or unexpected condition.
    /// </summary>
    Internal
}