using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ambev.DeveloperEvaluation.Functional.TestData;

public static class QueryStringHelper
{
    /// <summary>
    /// Converts an object's public properties to a query string.
    /// It respects the [FromQuery(Name = "...")] attribute.
    /// Skips properties decorated with [BindNever].
    /// </summary>
    public static string ToQueryString<T>(T obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));
        
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var queryParams = new List<string>();

        foreach (var prop in properties)
        {
            // Skip properties marked with [BindNever]
            if (prop.GetCustomAttributes(typeof(BindNeverAttribute), true).Any())
                continue;

            // If there's a [FromQuery] attribute with a specified Name, use it.
            var fromQueryAttr = prop.GetCustomAttribute<FromQueryAttribute>();
            string key = fromQueryAttr != null && !string.IsNullOrWhiteSpace(fromQueryAttr.Name)
                ? fromQueryAttr.Name
                : prop.Name;

            var value = prop.GetValue(obj);
            if (value != null)
            {
                queryParams.Add($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value.ToString()!)}");
            }
        }

        return queryParams.Any() ? "?" + string.Join("&", queryParams) : string.Empty;
    }
}