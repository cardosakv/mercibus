namespace Auth.Application.Common;

/// <summary>
/// API response messages.
/// </summary>
public static class Messages
{
    public const string UserRegistered = "User registered successfully.";
    public const string UserLoggedOut = "User logged out successfully.";
    public const string EmailConfirmationSent = "Email confirmation sent successfully.";
    public const string PasswordResetLinkSent = "Password reset link sent successfully.";
    public const string PasswordResetSuccess = "Password reset successfully.";
    public const string PasswordChanged = "Password changed successfully.";
    public const string EmailVerified = "Email verified successfully.";
    public const string UserInfoUpdated = "User info updated successfully.";
    public const string EmailAlreadyVerified = "Email is already verified.";
    public const string UserNotFound = "The specified user was not found.";
    public const string PasswordIncorrect = "The password provided is incorrect.";
    public const string UserForbidden = "You do not have permission to perform this action.";
    public const string UnexpectedError = "An unexpected error occurred while processing the request.";
    public const string RefreshTokenExpired = "The provided refresh token is no longer valid. Please log in again.";
}