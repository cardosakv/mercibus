namespace Auth.Application.DTOs;

/// <summary>
/// Standard 400 bad request response.
/// </summary>
public class BadRequestResponse
{
    /// <summary>
    /// Title message.
    /// </summary>
    public string Message { get; set; } = "One or more validation errors occurred.";

    /// <summary>
    /// List of request errors.
    /// </summary>
    public List<string> Errors { get; set; } = [];
}