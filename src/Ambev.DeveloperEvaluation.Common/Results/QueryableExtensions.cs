using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Common.Results;

public static class QueryableExtensions
{
    private static class FilterDefinitions
    {
        public const string NestedPropertyIndicator = ".";
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
            if (key.Contains(FilterDefinitions.MinRangeIndicator, StringComparison.OrdinalIgnoreCase))
            {
                string propertyName = key.Replace(FilterDefinitions.MinRangeIndicator, string.Empty);
                query = query.ApplyRangeFilter(propertyName, value, isMin: true);
            } else if (key.Contains(FilterDefinitions.MaxRangeIndicator, StringComparison.OrdinalIgnoreCase))
            {
                string propertyName = key.Replace(FilterDefinitions.MaxRangeIndicator, string.Empty);
                query = query.ApplyRangeFilter(propertyName, value, isMin: false);
            }
            else
            {
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

        // Handle nested properties using dot notation e.g: User.Address or Product.Rating 
        Expression? propertyAccess = QueryableExtensionsEnhancements.FindNestedPropertyExpression(parameter, propertyName);
        if (propertyAccess == null)
        {
            return query;
        }

        Type propertyType = propertyAccess.Type;
        Expression predicateBody;
        if (propertyType == typeof(string))
        {
            predicateBody = ApplyStringFilter<T>(filterValue, propertyAccess);
        }
        else if (propertyType.IsEnum)
        {
            var enumValue = QueryableExtensionsEnhancements.ConvertToEnumValue(filterValue, propertyType);
            if (enumValue == null)
            {
                return query;
            }
            predicateBody = Expression.Equal(propertyAccess, Expression.Constant(enumValue, propertyType));
        }
        else // Fallback
        {
            object? convertedValue;
            try
            {
                convertedValue = Convert.ChangeType(filterValue, propertyType);
            }
            catch
            {
                return query;
            }
            predicateBody = Expression.Equal(propertyAccess, Expression.Constant(convertedValue, propertyType));
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
            predicateBody = Expression.Equal(propertyAccess, Expression.Constant(filterValue));
        }
        else
        {
            ConstantExpression patternConstant = Expression.Constant(pattern);
            predicateBody = Expression.Call(propertyAccess, method, patternConstant);
        }

        return predicateBody;
    }
    private static IQueryable<T> ApplyRangeFilter<T>(this IQueryable<T> query, string propertyName, string filterValue, bool isMin)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

        // Handle nested properties using GetNestedPropertyExpression
        Expression? propertyAccess = QueryableExtensionsEnhancements.FindNestedPropertyExpression(parameter, propertyName);
        if (propertyAccess == null)
        {
            return query;
        }

        Type propertyType = propertyAccess.Type;
        // Only support range filtering on numeric or date types.
        if (!(propertyType == typeof(int) ||
              propertyType == typeof(decimal) ||
              propertyType == typeof(double) ||
              propertyType == typeof(DateTime)))
        {
            return query;
        }

        object? convertedValue;
        try
        {
            if (propertyType == typeof(decimal))
            {
                convertedValue = decimal.Parse(filterValue, CultureInfo.InvariantCulture);
            }
            else if (propertyType == typeof(double))
            {
                convertedValue = double.Parse(filterValue, CultureInfo.InvariantCulture);
            }
            else if (propertyType == typeof(int))
            {
                convertedValue = int.Parse(filterValue, CultureInfo.InvariantCulture);
            }
            else if (propertyType == typeof(DateTime))
            {
                convertedValue = DateTime.Parse(filterValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
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

        Expression constant = Expression.Constant(convertedValue, propertyType);
        Expression predicateBody = isMin
            ? Expression.GreaterThanOrEqual(propertyAccess, constant)
            : Expression.LessThanOrEqual(propertyAccess, constant);

        var lambda = Expression.Lambda<Func<T, bool>>(predicateBody, parameter);
        return query.Where(lambda);
    }
}