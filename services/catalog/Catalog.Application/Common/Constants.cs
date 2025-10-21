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
        public const int ReviewMinRating = 1;
        public const int ReviewMaxRating = 5;
        public const int ReviewMaxCommentLength = 500;
    }

    public static class CategoryValidation
    {
        public const int MaxNameLength = 100;
        public const int MaxDescriptionLength = 300;
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
        public const string CategoryNotFound = "category_not_found";
        public const string ParentCategoryNotFound = "parent_category_not_found";
        public const string CategoryInUse = "category_in_use";
        public const string BrandNotFound = "brand_not_found";
        public const string BrandInUse = "brand_in_use";
        public const string ImageNotFound = "image_not_found";
        public const string ImageNotInProduct = "image_not_in_product";
        public const string CommentTooLong = "comment_too_long";
        public const string InvalidRating = "invalid_rating";
        public const string ReviewNotFound = "review_not_found";
    }

    public static class BlobStorage
    {
        public const string ProductImagesContainer = "product-images";
        public const int BlobTokenExpirationHours = 2;
    }

    public static class Redis
    {
        public static TimeSpan CacheExpiration = TimeSpan.FromMinutes(10);
        public const string BrandPrefix = "brand-";
        public const string CategoryPrefix = "category-";
        public const string ProductPrefix = "product-";
        public const string ReviewPrefix = "-review-";
        public const string ImagePrefix = "-image-";
    }

    public static class Messages
    {
        public const string ExceptionOccurred = "An unexpected error has occurred.";
    }
}