using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Auth.Application.Common;

/// <summary>
/// Error codes for API responses.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum ErrorCode
{
    [EnumMember(Value = "validation_failed")]
    ValidationFailed,

    [EnumMember(Value = "request_body_empty")]
    RequestBodyEmpty,

    [EnumMember(Value = "username_required")]
    UsernameRequired,

    [EnumMember(Value = "username_invalid")]
    UsernameInvalid,

    [EnumMember(Value = "username_too_short")]
    UsernameTooShort,

    [EnumMember(Value = "username_too_long")]
    UsernameTooLong,

    [EnumMember(Value = "username_already_exists")]
    UsernameAlreadyExists,

    [EnumMember(Value = "email_required")] EmailRequired,

    [EnumMember(Value = "email_invalid")] EmailInvalid,

    [EnumMember(Value = "email_already_exists")]
    EmailAlreadyExists,

    [EnumMember(Value = "email_already_verified")]
    EmailAlreadyVerified,

    [EnumMember(Value = "password_required")]
    PasswordRequired,

    [EnumMember(Value = "password_too_short")]
    PasswordTooShort,

    [EnumMember(Value = "password_invalid")]
    PasswordInvalid,

    [EnumMember(Value = "password_mismatch")]
    PasswordMismatch,

    [EnumMember(Value = "password_already_set")]
    PasswordAlreadySet,

    [EnumMember(Value = "role_invalid")] RoleInvalid,

    [EnumMember(Value = "role_already_exists")]
    RoleAlreadyExists,

    [EnumMember(Value = "user_not_found")] UserNotFound,

    [EnumMember(Value = "user_no_role_assigned")]
    UserNoRoleAssigned,

    [EnumMember(Value = "user_already_in_role")]
    UserAlreadyInRole,

    [EnumMember(Value = "login_already_associated")]
    LoginAlreadyAssociated,

    [EnumMember(Value = "token_invalid")] TokenInvalid,

    [EnumMember(Value = "refresh_token_expired")]
    RefreshTokenExpired,

    [EnumMember(Value = "user_locked")] UserLocked,

    [EnumMember(Value = "unauthorized_request")]
    UnauthorizedRequest,

    [EnumMember(Value = "internal")] Internal
}