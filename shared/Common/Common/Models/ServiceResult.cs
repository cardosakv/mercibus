namespace Common.Models;

/// <summary>
/// Represents a standard service returned result.
/// </summary>
public class ServiceResult
{
    /// <summary>
    /// Indicates whether the process was successful.
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Message providing additional information.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Contains the data returned by the service.
    /// </summary>
    public object? Data { get; init; }

    /// <summary>
    /// Error type for the result.
    /// </summary>
    public string? ErrorType { get; init; }

    /// <summary>
    /// Error code for the result.
    /// </summary>
    public string? ErrorCode { get; init; }
}