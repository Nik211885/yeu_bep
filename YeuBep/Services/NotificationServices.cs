using Mapster;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.ViewModels.Notification;

namespace YeuBep.Services;

public class NotificationServices
{
    private readonly ILogger<NotificationServices> _logger;
    private readonly YeuBepDbContext _dbContext;

    public NotificationServices(ILogger<NotificationServices> logger, YeuBepDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task CreateNotificationAsync(CreateNotificationViewModel model)
    {
        var notification = model.Adapt<Notification>();
        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteNotificationAsync(params string[] ids)
    {
        await _dbContext.Notifications
            .Where(x=>((IEnumerable<string>)ids).Contains(x.Id)).ExecuteDeleteAsync();
    }
}