using Mapster;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Extensions;
using YeuBep.ViewModels;
using YeuBep.ViewModels.Notification;

namespace YeuBep.Queries;

public class NotificationQueries
{
    private readonly ILogger<NotificationQueries> _logger;
    private readonly YeuBepDbContext _dbContext;

    public NotificationQueries(ILogger<NotificationQueries> logger, YeuBepDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<PaginationViewModel<NotificationViewModel>> GetNotificationPagination(
        string userId,
        int pageNumber,
        int pageSize)
    {
        var result = await _dbContext.Notifications.AsNoTracking()
            .Where(x=>x.SendForUserId == userId)
            .OrderByDescending(x=>x.CreatedDate)
            .ProjectToType<NotificationViewModel>()
            .GetPaginationAsync(pageNumber, pageSize);
        return result;
    }
}