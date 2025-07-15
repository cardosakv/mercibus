using Auth.Application.Common;

namespace Auth.Application.Responses;

/// <summary>
/// Represents an error response from the API.
/// </summary>
public class ApiError
{
    /// <summary>
    /// Type of error.
    /// </summary>
    public required ErrorType Type { get; set; }

    /// <summary>
    /// Error code.
    /// </summary>
    public required ErrorCode Code { get; set; }

    /// <summary>
    /// Additional parameters related to the error, if any.
    /// </summary>
    public List<BadRequestParams>? Params { get; set; } = null;
}