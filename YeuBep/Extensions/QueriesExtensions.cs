using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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
                var matchedProp =
                    columnFilter.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
                if (matchedProp == null)
                    continue;
                var left = Expression.Property(parameter, matchedProp);
                Expression expr;
                if (matchedProp.PropertyType == typeof(string))
                {
                    var leftLower = Expression.Call(left, toLowerMethod);
                    var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
                    expr = Expression.Call(leftLower, containsMethod, Expression.Constant(filterValue));
                }
                else
                {
                    return queryable.AsEnumerable()
                        .Where(x =>
                        {
                            var val = matchedProp.GetValue(x)?.ToString()?.ToLower();
                            return val != null && val.Contains(filterValue);
                        })
                        .AsQueryable();
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
}