using Catalog.Domain.Enums;

namespace Catalog.Domain.Entities;

/// <summary>
/// Represents a product attribute, such as color, size, or weight.
/// </summary>
public class Attribute
{
    /// <summary>
    /// Unique identifier for the attribute.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Name of the attribute.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Value of the attribute.
    /// </summary>
    public required string Value { get; set; }

    /// <summary>
    /// Indicates whether the attribute is mandatory.
    /// </summary>
    public bool IsMandatory { get; set; }

    /// <summary>
    /// Type of the attribute.
    /// </summary>
    public required AttributeType Type { get; set; }

    /// <summary>
    /// Unit of measurement, if applicable.
    /// </summary>
    public string? Unit { get; set; }
}