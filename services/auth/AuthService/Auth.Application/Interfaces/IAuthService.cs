using Auth.Application.Common;
using Auth.Application.DTOs;

namespace Auth.Application.Interfaces
{
    /// <summary>
    /// Interface for authentication services.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="request">The registration request containing user email and password.</param>
        /// <returns><see cref="Response{T}"/> with a boolean value indicating whether the registration was successful.</returns>
        Task<Response> RegisterAsync(RegisterRequest request);
    }
}
