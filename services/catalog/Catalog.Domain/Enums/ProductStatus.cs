using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Catalog.Domain.Enums;

/// <summary>
/// Represents the visibility status of a product in the catalog.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum ProductStatus
{
    /// <summary>
    /// The product is not visible or available for purchase.
    /// </summary>
    [EnumMember(Value = "unlisted")] Unlisted,

    /// <summary>
    /// The product is visible and available for purchase.
    /// </summary>
    [EnumMember(Value = "listed")] Listed
}