namespace Auth.Application.Common;

public static class Constants
{
    public static class UserValidation
    {
        public const int UsernameMinLength = 5;
        public const int UsernameMaxLength = 20;
        public const string UsernamePattern = @"^[a-zA-Z0-9_]+$";

        public const int PasswordMinLength = 8;
        public const string PasswordPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$";

        public const int NameMinLength = 2;
        public const int NameMaxLength = 50;
    }

    public const string BlobStorageContainerName = "user-profiles";
    public const int BlobTokenExpirationHours = 2;
}