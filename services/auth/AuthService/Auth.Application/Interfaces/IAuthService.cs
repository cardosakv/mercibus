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
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration request containing user email and password.</param>
        /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
        Task<Response> RegisterAsync(RegisterRequest request);
        
        /// <summary>
        /// Logins a user.
        /// </summary>
        /// <param name="request">The registration request containing username and password.</param>
        /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
        Task<Response> LoginAsync(LoginRequest request);
    }
}
