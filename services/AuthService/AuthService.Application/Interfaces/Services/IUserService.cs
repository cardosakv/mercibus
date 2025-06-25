using AuthService.Application.Common;
using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces.Services
{
    /// <summary>
    /// Provides user services.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="request">User registration request.</param>
        /// <returns>
        /// API response indicating the success or failure.
        /// </returns>
        Task<Result<bool>> RegisterAsync(RegisterRequest request);
    }
}
