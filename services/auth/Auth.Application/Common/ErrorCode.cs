namespace Auth.Application.Common;

/// <summary>
/// Error codes for API responses.
/// </summary>
public static class ErrorCode
{
    public const string ValidationFailed = "validation_failed";

    public const string UsernameRequired = "username_required";
    public const string UsernameInvalid = "username_invalid";
    public const string UsernameTooShort = "username_too_short";
    public const string UsernameTooLong = "username_too_long";
    public const string UsernameAlreadyExists = "username_already_exists";

    public const string NameTooShort = "name_too_short";
    public const string NameTooLong = "name_too_long";

    public const string EmailRequired = "email_required";
    public const string EmailInvalid = "email_invalid";
    public const string EmailAlreadyExists = "email_already_exists";
    public const string EmailAlreadyVerified = "email_already_verified";

    public const string PasswordRequired = "password_required";
    public const string PasswordTooShort = "password_too_short";
    public const string PasswordInvalid = "password_invalid";
    public const string PasswordMismatch = "password_mismatch";
    public const string PasswordAlreadySet = "password_already_set";

    public const string RoleInvalid = "role_invalid";
    public const string RoleAlreadyExists = "role_already_exists";

    public const string UserNotFound = "user_not_found";
    public const string UserNoRoleAssigned = "user_no_role_assigned";
    public const string UserAlreadyInRole = "user_already_in_role";

    public const string LoginAlreadyAssociated = "login_already_associated";
    public const string TokenInvalid = "token_invalid";
    public const string RefreshTokenExpired = "refresh_token_expired";
    public const string UserLocked = "user_locked";

    public const string Unauthorized = "unauthorized";
    public const string Internal = "internal";
}