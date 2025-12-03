using Microsoft.EntityFrameworkCore;
using YeuBep.ViewModels;

namespace YeuBep.Extensions;

public static class QueriesExtensions
{
    public static async Task<PaginationViewModel<TResponse>> GetPaginationAsync<TResponse>(
        this IQueryable<TResponse> queryable,
        int pageNumber,
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

}