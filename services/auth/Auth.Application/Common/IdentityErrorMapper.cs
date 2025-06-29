namespace Auth.Application.Common;

/// <summary>
/// Utility mapper for ASP.Net Core Identity errors.
/// </summary>
public static class IdentityErrorMapper
{
    public static ErrorType MapToErrorType(string code)
    {
        return code switch
        {
            // Conflict (409)
            "DuplicateEmail" => ErrorType.Conflict,
            "DuplicateUserName" => ErrorType.Conflict,
            "DuplicateRoleName" => ErrorType.Conflict,
            "UserAlreadyHasPassword" => ErrorType.Conflict,
            "UserAlreadyInRole" => ErrorType.Conflict,
            "LoginAlreadyAssociated" => ErrorType.Conflict,
            "ConcurrencyFailure" => ErrorType.Conflict,

            // BadRequest (400)
            "InvalidEmail" => ErrorType.BadRequest,
            "InvalidUserName" => ErrorType.BadRequest,
            "InvalidRoleName" => ErrorType.BadRequest,
            "InvalidToken" => ErrorType.BadRequest,
            "PasswordTooShort" => ErrorType.BadRequest,
            "PasswordRequiresNonAlphanumeric" => ErrorType.BadRequest,
            "PasswordRequiresDigit" => ErrorType.BadRequest,
            "PasswordRequiresLower" => ErrorType.BadRequest,
            "PasswordRequiresUpper" => ErrorType.BadRequest,
            "PasswordRequiresUniqueChars" => ErrorType.BadRequest,
            "PasswordMismatch" => ErrorType.BadRequest,

            // Unauthorized (401)
            "UserLockoutNotEnabled" => ErrorType.Unauthorized,
            "InvalidPasswordHasherCompatibilityMode" => ErrorType.Unauthorized,
            "InvalidPasswordHasherIterationCount" => ErrorType.Unauthorized,

            // Locked (423)
            "UserLockedOut" => ErrorType.Locked,

            // NotFound (404)
            "RoleNotFound" => ErrorType.NotFound,
            "UserNameNotFound" => ErrorType.NotFound,
            "UserNotInRole" => ErrorType.NotFound,

            // InternalServerError (500)
            "DefaultError" => ErrorType.Internal,
            "MustCallAddIdentity" => ErrorType.Internal,
            "NoTokenProvider" => ErrorType.Internal,
            "NullSecurityStamp" => ErrorType.Internal,
            "InvalidManagerType" => ErrorType.Internal,
            "StoreNotIQueryableRoleStore" => ErrorType.Internal,
            "StoreNotIQueryableUserStore" => ErrorType.Internal,
            "StoreNotIRoleClaimStore" => ErrorType.Internal,
            "StoreNotIUserAuthenticationTokenStore" => ErrorType.Internal,
            "StoreNotIUserClaimStore" => ErrorType.Internal,
            "StoreNotIUserConfirmationStore" => ErrorType.Internal,
            "StoreNotIUserEmailStore" => ErrorType.Internal,
            "StoreNotIUserLockoutStore" => ErrorType.Internal,
            "StoreNotIUserLoginStore" => ErrorType.Internal,
            "StoreNotIUserPasswordStore" => ErrorType.Internal,
            "StoreNotIUserPhoneNumberStore" => ErrorType.Internal,
            "StoreNotIUserRoleStore" => ErrorType.Internal,
            "StoreNotIUserSecurityStampStore" => ErrorType.Internal,
            "StoreNotIUserAuthenticatorKeyStore" => ErrorType.Internal,
            "StoreNotIUserTwoFactorStore" => ErrorType.Internal,
            "StoreNotIUserTwoFactorRecoveryCodeStore" => ErrorType.Internal,
            "StoreNotIProtectedUserStore" => ErrorType.Internal,
            "NoPersonalDataProtector" => ErrorType.Internal,
            "NoRoleType" => ErrorType.Internal,
            "RecoveryCodeRedemptionFailed" => ErrorType.Internal,

            // Fallback
            _ => ErrorType.Internal
        };
    }
}