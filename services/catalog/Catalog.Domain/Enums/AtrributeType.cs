namespace Catalog.Domain.Enums
{
    /// <summary>
    /// Defines the data type of product attribute.
    /// </summary>
    public enum AttributeType
    {
        /// <summary>
        /// Integer value.
        /// </summary>
        Int,

        /// <summary>
        /// Text value.
        /// </summary>
        String,

        /// <summary>
        /// Value selected from a predefined list.
        /// </summary>
        Enum,

        /// <summary>
        /// Floating-point number.
        /// </summary>
        Float,

        /// <summary>
        /// Date and time value.
        /// </summary>
        Timestamp
    }
}