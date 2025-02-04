using System.Linq.Expressions;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Common.Results;

public static class QueryableExtensionsEnhancements
{
    /// <summary>
    /// Gets a property expression for nested properties using dot notation.
    /// </summary>
    public static Expression? FindNestedPropertyExpression(ParameterExpression parameter, string propertyPath)
    {
        var parts = propertyPath.Split('.');
        Expression propertyAccess = parameter;

        foreach (var part in parts)
        {

            PropertyInfo? property = propertyAccess.Type.GetProperties()
                .FirstOrDefault(prop => prop.Name.Equals(part, StringComparison.OrdinalIgnoreCase));

            if (property == null)
            {
                return null;
            }

            propertyAccess = Expression.Property(propertyAccess, property);
        }

        return propertyAccess;
    }

    /// <summary>
    /// Converts a string value to an enum value
    /// </summary>
    public static object? ConvertToEnumValue(string value, Type enumType)
    {
        if (!enumType.IsEnum)
        {
            throw new ArgumentException($"Type {enumType.Name} is not an enum");
        }

        try
        {
            return Enum.Parse(enumType, value, true);
        }
        catch
        {
            return null;
        }
    }
}