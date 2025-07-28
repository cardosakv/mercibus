namespace Catalog.Application.Common;

/// <summary>
/// Contains constants used across the application.
/// </summary>
public static class Constants
{
    public static class ProductValidation
    {
        public const int MaxNameLength = 100;
        public const int MaxDescriptionLength = 300;
        public const int MaxSkuLength = 100;
    }

    public static class ErrorCode
    {
        public const string ProductNotFound = "product_not_found";
    }
}