namespace Auth.Application.Responses;

/// <summary>
/// Represents a successful API response containing data of type T.
/// </summary>
public class ApiSuccessResponse
{
    /// <summary>
    /// The data returned. Set to null if no data.
    /// </summary>
    public object? Data { get; set; }
}