using Auth.Application.DTOs;
using Auth.Domain.Entities;

namespace Auth.Application.Interfaces
{
    /// <summary>
    /// Interface for token services.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates JWT token.
        /// </summary>
        /// <param name="user">User entity.</param>
        /// <param name="role">User role.</param>
        /// <returns>Generated <see cref="AuthToken"/>.</returns>
        AuthToken CreateToken(User user, string role);
    }
}
