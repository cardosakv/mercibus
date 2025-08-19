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
        public const string NameRequired = "name_required";
        public const string NameTooLong = "name_too_long";
        public const string DescriptionTooLong = "description_too_long";
        public const string SkuRequired = "sku_required";
        public const string PriceRequired = "price_required";
        public const string StockQuantityRequired = "stock_quantity_required";
        public const string PriceInvalid = "price_invalid";
    }
}