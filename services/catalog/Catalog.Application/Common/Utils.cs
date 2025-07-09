using System.Reflection;
using System.Runtime.Serialization;

namespace Catalog.Application.Common;

/// <summary>
/// Utility class for common operations.
/// </summary>
public class Utils
{
    /// <summary>
    /// Gets the EnumMember value of an enum value.
    /// </summary>
    public static string GetEnumMemberValue(Enum value)
    {
        var member = value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault();

        return member?
            .GetCustomAttribute<EnumMemberAttribute>()?
            .Value ?? value.ToString();
    }

    /// <summary>
    /// Parses a string to get the corresponding enum value based on EnumMember attributes.
    /// </summary>
    public static T ParseEnumMemberValue<T>(string value) where T : struct, Enum
    {
        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attr = field.GetCustomAttribute<EnumMemberAttribute>();
            if (attr?.Value?.Equals(value, StringComparison.OrdinalIgnoreCase) == true)
            {
                return (T)field.GetValue(null)!;
            }
        }

        throw new ArgumentException($"'{value}' is not a valid value for enum {typeof(T).Name}");
    }
}