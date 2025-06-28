using System.Security.Cryptography;
using System.Text;

namespace Auth.Application.Common
{
    /// <summary>
    /// Common utility methods.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Hashes a string using SHA.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>Hashed value of the string.</returns>
        public static string HashString(string value)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Generates a random generated string.
        /// </summary>
        /// <returns>Random string.</returns>
        public static string GenerateRandomString()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[250];   
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
