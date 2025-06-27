namespace Auth.Application.Common
{
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
    }

    /// <summary>
    /// Represents a service returned result with data.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    public class Response<T> : Response
    {
        /// <summary>
        /// Contains the data returned by the service.
        /// </summary>
        public T? Data { get; set; }
    }
}
