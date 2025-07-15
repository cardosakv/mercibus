namespace Common.Responses;

/// <summary>
/// Represents an error response from the API.
/// </summary>
public class ApiError
{
    /// <summary>
    /// Type of error.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Error code.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Additional parameters related to the error, if any.
    /// </summary>
    public List<BadRequestParams>? Params { get; set; }
}