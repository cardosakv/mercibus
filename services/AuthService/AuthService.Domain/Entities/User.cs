namespace AuthService.Domain.Entities
{
    /// <summary>
    /// User entity.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password in hash.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Created date and time.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
