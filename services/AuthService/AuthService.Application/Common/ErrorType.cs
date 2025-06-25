namespace AuthService.Application.Common
{
    /// <summary>
    /// Error types for service responses.
    /// </summary>
    public enum ErrorType
    {
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
        /// Internal server error or unexpected condition.
        /// </summary>
        InternalError
    }
}
