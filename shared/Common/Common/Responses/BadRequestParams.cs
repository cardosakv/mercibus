namespace Common.Responses;

/// <summary>
/// Represents parameters for a bad request error.
/// </summary>
public class BadRequestParams
{
    /// <summary>
    /// The field that caused the error.
    /// </summary>
    public required string Field { get; set; }

    /// <summary>
    /// The error code associated with the bad request parameter.
    /// </summary>
    public required string Code { get; set; }
}