using Microsoft.EntityFrameworkCore;
using YeuBep.ViewModels;

namespace YeuBep.Extensions;

public class QueriesExtensions
{
    public static async Task<PaginationViewModel<TResponse>> GetPaginationAsync<TResponse>(
        IQueryable<TResponse> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        var totalPage = await queryable.CountAsync(cancellationToken);
        var paginationItem = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return new PaginationViewModel<TResponse>(paginationItem, pageNumber, pageSize, totalPage);
    }
}