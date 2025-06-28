namespace Auth.Application.DTOs
{
    /// <summary>
    /// Refresh access token request.
    /// </summary>
    public class RefreshRequest
    {
        /// <summary>
        /// Refresh token string.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
