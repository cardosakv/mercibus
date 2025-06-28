using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Entities
{
    /// <summary>
    /// Represents a user in the authentication system.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// User display name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Created date and time of the user.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
