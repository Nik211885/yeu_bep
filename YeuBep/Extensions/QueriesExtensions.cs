using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using YeuBep.Attributes.Table;
using YeuBep.ViewModels;

namespace YeuBep.Extensions;

public static class QueriesExtensions
{
    extension<TResponse>(IQueryable<TResponse> queryable)
    {
        public async Task<PaginationViewModel<TResponse>> GetPaginationAsync(int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default
        )
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            var skip = (pageNumber - 1) * pageSize;
            var totalCount = await queryable.CountAsync(cancellationToken);
            var items = await queryable
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            return new PaginationViewModel<TResponse>(items, pageNumber, pageSize, totalCount);
        }

        
        public IQueryable<TResponse> WhereEqualFilterValue(Dictionary<string, string>? filterEqualValue)
        {
            if (filterEqualValue is null || filterEqualValue.Count == 0)
                return queryable;
            var typeOfViewModel = typeof(TResponse);
            var columnFilter = typeOfViewModel.GetProperties()
                .Where(prop => !string.IsNullOrWhiteSpace(prop.GetNameColumn()))
                .ToList();
            if (!columnFilter.Any())
                return queryable;
            var parameter = Expression.Parameter(typeOfViewModel, "x");
            Expression? finalExpression = null;
            var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
            foreach (var kv in filterEqualValue)
            {
                var key = kv.Key;
                var filterValue = kv.Value?.ToLower();
                
                if (string.IsNullOrWhiteSpace(filterValue))
                    continue;
                var matchedProp = columnFilter.FirstOrDefault(p => 
                    p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
                if (matchedProp == null)
                    continue;
                var left = Expression.Property(parameter, matchedProp);
                Expression expr;
                if (matchedProp.PropertyType == typeof(string))
                {
                    var leftLower = Expression.Call(left, toLowerMethod);
                    var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;
                    expr = Expression.Call(leftLower, containsMethod, Expression.Constant(filterValue));
                }
                else
                {
                    var underlyingType = Nullable.GetUnderlyingType(matchedProp.PropertyType) ?? matchedProp.PropertyType;
                    if (underlyingType == typeof(DateTime) || underlyingType == typeof(DateTimeOffset))
                    {
                        expr = BuildDateTimeExpression(left, matchedProp.PropertyType, underlyingType, filterValue);
                    }
                    else if (underlyingType.IsEnum)
                    {
                        expr = BuildEnumExpression(left, underlyingType, matchedProp.PropertyType, filterValue);
                    }
                    else
                    {
                        expr = BuildDefaultExpression(left, matchedProp.PropertyType, filterValue, toLowerMethod);
                    }
                }
                
                finalExpression = finalExpression == null 
                    ? expr 
                    : Expression.AndAlso(finalExpression, expr);
            }
            
            if (finalExpression == null)
                return queryable;
            
            var lambda = Expression.Lambda<Func<TResponse, bool>>(finalExpression, parameter);
            return queryable.Where(lambda);
        }

        public IQueryable<TResponse> OrderByDescendingViewModel(object orderByValue)
        {
            throw new NotImplementedException();
        }
    }
    private static Expression BuildEnumExpression(Expression left, Type underlyingEnumType, Type propertyType, string filterValue)
    {
        var enumValues = Enum.GetValues(underlyingEnumType);
        var matchedEnumValues = new List<object>();
        foreach (var enumValue in enumValues)
        {
            var memberInfo = underlyingEnumType.GetMember(enumValue.ToString()!).FirstOrDefault();
            if (memberInfo != null)
            {
                var attr = memberInfo.GetCustomAttribute<EnumColumnTableAttribute>();
                var displayText = attr?.ColumnName ?? enumValue.ToString()!;
                if (displayText.ToLower().Contains(filterValue))
                {
                    matchedEnumValues.Add(enumValue);
                }
            }
        }
        if (matchedEnumValues.Count == 0)
        {
            return Expression.Constant(false);
        }
        Expression? enumExpr = null;
        if (Nullable.GetUnderlyingType(propertyType) != null)
        {
            var hasValueProp = Expression.Property(left, "HasValue");
            var valueProp = Expression.Property(left, "Value");
            enumExpr = matchedEnumValues
                .Select(enumValue => Expression.Equal(valueProp, Expression.Constant(enumValue, underlyingEnumType)))
                .Aggregate(enumExpr, (current, equalExpr) => current == null ? equalExpr : Expression.OrElse(current, equalExpr));
            return Expression.AndAlso(hasValueProp, enumExpr!);
        }
        foreach (var enumValue in matchedEnumValues)
        {
            var equalExpr = Expression.Equal(left, Expression.Constant(enumValue, underlyingEnumType));
            enumExpr = enumExpr == null ? equalExpr : Expression.OrElse(enumExpr, equalExpr);
        }
        return enumExpr!;
    }
    private static Expression BuildDefaultExpression(Expression left, Type propertyType, string filterValue, MethodInfo toLowerMethod)
    {
        var toStringMethod = typeof(object).GetMethod(nameof(ToString))!;
        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;
        if (propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) != null)
        {
            var hasValueProp = Expression.Property(left, "HasValue");
            var valueProp = Expression.Property(left, "Value");
            var valueToString = Expression.Call(valueProp, toStringMethod);
            var valueToLower = Expression.Call(valueToString, toLowerMethod);
            var containsExpr = Expression.Call(valueToLower, containsMethod, Expression.Constant(filterValue));
            return Expression.AndAlso(hasValueProp, containsExpr);
        }
        var toStringExpr = Expression.Call(left, toStringMethod);
        var toLower = Expression.Call(toStringExpr, toLowerMethod);
        return Expression.Call(toLower, containsMethod, Expression.Constant(filterValue));
    }
    private static Expression BuildDateTimeExpression(Expression left, Type propertyType, Type underlyingType, string filterValue)
    {
        var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;
        var dateParts = filterValue.Split('/');
        Expression? dateExpr;
        if (Nullable.GetUnderlyingType(propertyType) != null)
        {
            var hasValueProp = Expression.Property(left, "HasValue");
            var valueProp = Expression.Property(left, "Value");
            dateExpr = BuildDateComponentsExpression(valueProp, underlyingType, dateParts);
            return dateExpr != null ? Expression.AndAlso(hasValueProp, dateExpr) : BuildDateTimeFallbackExpression(left, propertyType, filterValue, toLowerMethod, containsMethod);
        }
        dateExpr = BuildDateComponentsExpression(left, underlyingType, dateParts);
        return dateExpr ?? BuildDateTimeFallbackExpression(left, propertyType, filterValue, toLowerMethod, containsMethod);
    }

    private static Expression? BuildDateComponentsExpression(Expression dateProperty, Type dateType, string[] dateParts)
    {
        var dayProp = Expression.Property(dateProperty, "Day");
        var monthProp = Expression.Property(dateProperty, "Month");
        var yearProp = Expression.Property(dateProperty, "Year");
        var toStringMethod = typeof(int).GetMethod(nameof(int.ToString), Type.EmptyTypes)!;
        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;
        Expression? finalExpr = null;
        if (dateParts.Length >= 1 && !string.IsNullOrWhiteSpace(dateParts[0]))
        {
            var dayStr = Expression.Call(dayProp, toStringMethod);
            var dayContains = Expression.Call(dayStr, containsMethod, Expression.Constant(dateParts[0]));
            finalExpr = dayContains;
        }
        if (dateParts.Length >= 2 && !string.IsNullOrWhiteSpace(dateParts[1]))
        {
            var monthStr = Expression.Call(monthProp, toStringMethod);
            var monthContains = Expression.Call(monthStr, containsMethod, Expression.Constant(dateParts[1]));
            finalExpr = finalExpr == null ? monthContains : Expression.AndAlso(finalExpr, monthContains);
        }
        if (dateParts.Length < 3 || string.IsNullOrWhiteSpace(dateParts[2])) return finalExpr;
        var yearStr = Expression.Call(yearProp, toStringMethod);
        var yearContains = Expression.Call(yearStr, containsMethod, Expression.Constant(dateParts[2]));
        finalExpr = finalExpr == null ? yearContains : Expression.AndAlso(finalExpr, yearContains);
        return finalExpr;
    }

    private static Expression BuildDateTimeFallbackExpression(Expression left, Type propertyType, string filterValue, 
        MethodInfo toLowerMethod, MethodInfo containsMethod)
    {
        var toStringMethod = typeof(object).GetMethod(nameof(ToString))!;
        if (Nullable.GetUnderlyingType(propertyType) != null)
        {
            var hasValueProp = Expression.Property(left, "HasValue");
            var valueProp = Expression.Property(left, "Value");
            var valueToString = Expression.Call(valueProp, toStringMethod);
            var valueToLower = Expression.Call(valueToString, toLowerMethod);
            var containsExpr = Expression.Call(valueToLower, containsMethod, Expression.Constant(filterValue));
            return Expression.AndAlso(hasValueProp, containsExpr);
        }
        var toStringExpr = Expression.Call(left, toStringMethod);
        var toLower = Expression.Call(toStringExpr, toLowerMethod);
        return Expression.Call(toLower, containsMethod, Expression.Constant(filterValue));
    }
}