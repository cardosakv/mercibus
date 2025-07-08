namespace Catalog.Domain.Enums;

/// <summary>
/// Represents the visibility status of a product in the catalog.
/// </summary>
public enum ProductStatus
{
    /// <summary>
    /// The product is not visible or available for purchase.
    /// </summary>
    Unlisted,

    /// <summary>
    /// The product is visible and available for purchase.
    /// </summary>
    Listed
}