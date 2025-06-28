namespace Auth.Application.Common;

/// <summary>
/// Represents a standard service returned result.
/// </summary>
public class Response
{
    /// <summary>
    /// Indicates whether the process was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Message providing additional information.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Error type for the result.
    /// </summary>
    public ErrorType? ErrorType { get; set; }

    /// <summary>
    /// Contains the data returned by the service.
    /// </summary>
    public object? Data { get; set; }
}