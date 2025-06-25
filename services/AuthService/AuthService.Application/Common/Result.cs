using System.Net;

namespace AuthService.Application.Common
{
    /// <summary>
    /// Represents a standard service returned result.
    /// </summary>
    /// <typeparam name="T">Result data type.</typeparam>
    public class Result<T>
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
        /// Contains the data returned by the service.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Error type for the result.
        /// </summary>
        public ErrorType ErrorType { get; set; }
    }
}
