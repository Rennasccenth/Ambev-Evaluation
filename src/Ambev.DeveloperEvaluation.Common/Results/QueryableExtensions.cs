using System.Linq.Expressions;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Common.Results;

public static class QueryableExtensions
{
    private static class FilterDefinitions
    {
        public const string MinRangeIndicator = "_min";
        public const string MaxRangeIndicator = "_max";
        public const char WildcardOperator = '*';

        public static class StringMethodsName
        {
            public const string Contains = "Contains";
            public const string StartsWith = "StartsWith";
            public const string EndsWith = "EndsWith";
        }
    } 
    
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IReadOnlyDictionary<string, string> filters)
    {
        foreach (var (key, value) in filters)
        {
            if (key.StartsWith(FilterDefinitions.MinRangeIndicator, StringComparison.OrdinalIgnoreCase) ||
                key.StartsWith(FilterDefinitions.MaxRangeIndicator, StringComparison.OrdinalIgnoreCase))
            {
                bool isMin = key.StartsWith(FilterDefinitions.MinRangeIndicator, StringComparison.OrdinalIgnoreCase);
                string propertyName = key[4..];

                query = query.ApplyRangeFilter(propertyName, value, isMin);
            }
            else
            {
                // Standard (exact or wildcard) filter.
                query = query.ApplyValueFilter(key, value);
            }
        }

        return query;
    }

    /// <summary>
    /// Applies an equality (or wildcard for strings) filter on the specified property.
    /// </summary>
    private static IQueryable<T> ApplyValueFilter<T>(this IQueryable<T> query, string propertyName, string filterValue)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        // Try to get the property (case-insensitive)
        PropertyInfo? property = typeof(T).GetProperties()
            .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

        if (property == null)
        {
            // Unknown property – ignoring it.
            return query;
        }

        Expression propertyAccess = Expression.Property(parameter, property);
        Expression predicateBody;

        if (property.PropertyType == typeof(string))
        {
            predicateBody = ApplyStringFilter<T>(filterValue, propertyAccess);
        }
        else
        {
            // For non-string properties, convert the value and use equality.
            object? convertedValue;
            try
            {
                convertedValue = Convert.ChangeType(filterValue, property.PropertyType);
            }
            catch
            {
                // If conversion fails, skip this filter.
                return query;
            }
            predicateBody = Expression.Equal(propertyAccess, Expression.Constant(convertedValue, property.PropertyType));
        }

        var lambda = Expression.Lambda<Func<T, bool>>(predicateBody, parameter);
        return query.Where(lambda);
    }

    private static Expression ApplyStringFilter<T>(string filterValue, Expression propertyAccess)
    {
        Expression predicateBody = filterValue.Contains(FilterDefinitions.WildcardOperator) // Detect 'Like' comparisons 
            ? ApplyLikeExpression(filterValue, propertyAccess) 
            : Expression.Equal(propertyAccess, Expression.Constant(filterValue));

        return predicateBody;
    }

    private static Expression ApplyLikeExpression(string filterValue, Expression propertyAccess)
    {
        // Remove the '*' before comparing.
        string pattern = filterValue.Replace(FilterDefinitions.WildcardOperator.ToString(), string.Empty);
        MethodInfo? method = null;

        if (filterValue.StartsWith(FilterDefinitions.WildcardOperator) &&
            filterValue.EndsWith(FilterDefinitions.WildcardOperator))
        {
            method = typeof(string).GetMethod(FilterDefinitions.StringMethodsName.Contains, [typeof(string)]);
        }
        else if (filterValue.StartsWith(FilterDefinitions.WildcardOperator))
        {
            method = typeof(string).GetMethod(FilterDefinitions.StringMethodsName.EndsWith, [typeof(string)]);
        }
        else if (filterValue.EndsWith(FilterDefinitions.WildcardOperator))
        {
            method = typeof(string).GetMethod(FilterDefinitions.StringMethodsName.StartsWith, [typeof(string)]);
        }

        Expression predicateBody;
        if (method == null)
        {
            // Fallback to equality
            predicateBody = Expression.Equal(propertyAccess, Expression.Constant(filterValue));
        }
        else
        {
            ConstantExpression patternConstant = Expression.Constant(pattern);
            predicateBody = Expression.Call(propertyAccess, method, patternConstant);
        }

        return predicateBody;
    }

    /// <summary>
    /// Applies a range filter (min or max) for numeric or date properties.
    /// </summary>
    private static IQueryable<T> ApplyRangeFilter<T>(this IQueryable<T> query, string propertyName, string filterValue, bool isMin)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        // Try to get the property (case-insensitive)
        PropertyInfo? property = typeof(T).GetProperties()
            .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

        if (property == null)
        {
            return query;
        }

        // Only support range filtering on numeric or date types.
        if (!(property.PropertyType == typeof(int)     ||
              property.PropertyType == typeof(decimal) ||
              property.PropertyType == typeof(double)  ||
              property.PropertyType == typeof(DateTime)))
        {
            return query;
        }

        object? convertedValue;
        try
        {
            if (property.PropertyType == typeof(decimal))
            {
                convertedValue = decimal.Parse(filterValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (property.PropertyType == typeof(double))
            {
                convertedValue = double.Parse(filterValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (property.PropertyType == typeof(int))
            {
                convertedValue = int.Parse(filterValue, System.Globalization.CultureInfo.InvariantCulture);

            } else if (property.PropertyType == typeof(DateTime))
            {
                convertedValue = DateTime.Parse(filterValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(filterValue), "Unsupported property type for range filter."); 
            }
        }
        catch
        {
            return query;
        }

        MemberExpression propertyAccess = Expression.Property(parameter, property);
        Expression constant = Expression.Constant(convertedValue, property.PropertyType);

        Expression predicateBody = isMin
            ? Expression.GreaterThanOrEqual(propertyAccess, constant)
            : Expression.LessThanOrEqual(propertyAccess, constant);

        var lambda = Expression.Lambda<Func<T, bool>>(predicateBody, parameter);
        return query.Where(lambda);
    }
}