using Auth.Domain.Entities;

namespace Auth.Application.Interfaces
{
    /// <summary>
    /// Interface for refresh token repository.
    /// </summary>
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Adds a new refresh token to persistence.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>Refresh token string.</returns>
        Task<string> CreateTokenAsync(string userId);
        
        /// <summary>
        /// Gets the persisted refresh token entity.
        /// </summary>
        /// <param name="tokenString">Refresh token string to check.</param>
        /// <returns>Refresh token entity.</returns>
        Task<RefreshToken?> RetrieveTokenAsync(string tokenString);
        
        /// <summary>
        /// Invalidates the old refresh token and generates a new one.
        /// </summary>
        /// <param name="refreshToken">Previous refresh token.</param>
        /// <returns>New refresh token string.</returns>
        Task<string> RotateTokenAsync(RefreshToken refreshToken);
    }
}
