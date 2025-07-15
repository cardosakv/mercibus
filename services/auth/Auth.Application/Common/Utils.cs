using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Common.Constants;

namespace Auth.Application.Common;

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
        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = SHA256.HashData(bytes);
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

    /// <summary>
    /// Gets the enum member value for a given enum value.
    /// </summary>
    /// <param name="value">Enum value.</param>
    /// <returns>Enum member string value.</returns>
    public static string GetEnumMemberValue(this Enum value)
    {
        var type = value.GetType();
        var member = type.GetMember(value.ToString()).FirstOrDefault();

        if (member == null)
        {
            return value.ToString();
        }

        var enumMemberAttr = member.GetCustomAttribute<EnumMemberAttribute>();
        return enumMemberAttr?.Value ?? value.ToString();
    }

    /// <summary>
    /// Maps an ASP.Net Core Identity error code to a standardized ErrorType.
    /// </summary>
    public static string IdentityErrorToType(string code)
    {
        return code switch
        {
            // Conflict (409)
            "DuplicateEmail" => ErrorType.ConflictError,
            "DuplicateUserName" => ErrorType.ConflictError,
            "DuplicateRoleName" => ErrorType.ConflictError,
            "UserAlreadyHasPassword" => ErrorType.ConflictError,
            "UserAlreadyInRole" => ErrorType.ConflictError,
            "LoginAlreadyAssociated" => ErrorType.ConflictError,

            // BadRequest (400)
            "InvalidEmail" => ErrorType.InvalidRequestError,
            "InvalidUserName" => ErrorType.InvalidRequestError,
            "InvalidRoleName" => ErrorType.InvalidRequestError,
            "InvalidToken" => ErrorType.InvalidRequestError,
            "PasswordTooShort" => ErrorType.InvalidRequestError,
            "PasswordRequiresNonAlphanumeric" => ErrorType.InvalidRequestError,
            "PasswordRequiresDigit" => ErrorType.InvalidRequestError,
            "PasswordRequiresLower" => ErrorType.InvalidRequestError,
            "PasswordRequiresUpper" => ErrorType.InvalidRequestError,
            "PasswordRequiresUniqueChars" => ErrorType.InvalidRequestError,
            "PasswordMismatch" => ErrorType.InvalidRequestError,
            "RoleNotFound" => ErrorType.InvalidRequestError,
            "UserNameNotFound" => ErrorType.InvalidRequestError,
            "UserNotInRole" => ErrorType.InvalidRequestError,

            // Unauthorized (401)
            "UserLockoutNotEnabled" => ErrorType.AuthenticationError,
            "InvalidPasswordHasherCompatibilityMode" => ErrorType.AuthenticationError,
            "InvalidPasswordHasherIterationCount" => ErrorType.AuthenticationError,

            // Locked (423)
            "UserLockedOut" => ErrorType.LockedError,

            // Fallback
            _ => ErrorType.ApiError
        };
    }

    /// <summary>
    /// Maps an ASP.Net Core Identity error code to a standardized ErrorCode.
    /// </summary>
    public static string IdentityErrorToCode(string code)
    {
        return code switch
        {
            // Conflict (409)
            "DuplicateEmail" => ErrorCode.EmailAlreadyExists,
            "DuplicateUserName" => ErrorCode.UsernameAlreadyExists,
            "DuplicateRoleName" => ErrorCode.RoleAlreadyExists,
            "UserAlreadyHasPassword" => ErrorCode.PasswordAlreadySet,
            "UserAlreadyInRole" => ErrorCode.UserAlreadyInRole,
            "LoginAlreadyAssociated" => ErrorCode.LoginAlreadyAssociated,

            // BadRequest (400)
            "InvalidEmail" => ErrorCode.EmailInvalid,
            "InvalidUserName" => ErrorCode.UsernameInvalid,
            "InvalidRoleName" => ErrorCode.RoleInvalid,
            "InvalidToken" => ErrorCode.TokenInvalid,
            "PasswordTooShort" => ErrorCode.PasswordTooShort,
            "PasswordRequiresNonAlphanumeric" => ErrorCode.PasswordInvalid,
            "PasswordRequiresDigit" => ErrorCode.PasswordInvalid,
            "PasswordRequiresLower" => ErrorCode.PasswordInvalid,
            "PasswordRequiresUpper" => ErrorCode.PasswordInvalid,
            "PasswordRequiresUniqueChars" => ErrorCode.PasswordInvalid,
            "PasswordMismatch" => ErrorCode.PasswordMismatch,

            // Unauthorized (401)
            "UserLockoutNotEnabled" => ErrorCode.Unauthorized,
            "InvalidPasswordHasherCompatibilityMode" => ErrorCode.Unauthorized,
            "InvalidPasswordHasherIterationCount" => ErrorCode.Unauthorized,

            // Locked (423)
            "UserLockedOut" => ErrorCode.UserLocked,

            // NotFound (404)
            "RoleNotFound" => ErrorCode.UserNotFound,
            "UserNameNotFound" => ErrorCode.UserNotFound,
            "UserNotInRole" => ErrorCode.UserNotFound,

            // Fallback
            _ => ErrorCode.Internal
        };
    }
}