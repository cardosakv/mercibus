namespace Auth.Application.Responses;

/// <summary>
/// Represents an error response from the API.
/// </summary>
public class ApiErrorResponse
{
    /// <summary>
    /// The error body.
    /// </summary>
    public required ApiError Error { get; set; }
}