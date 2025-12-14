using Microsoft.AspNetCore.Mvc;
using YeuBep.Extensions;
using YeuBep.Queries;
using YeuBep.ViewModels;
using YeuBep.ViewModels.Notification;

namespace YeuBep.Controllers.Apis;
[ApiController]
[Route("api/notification")]
public class NotificationApiController : ControllerBase
{
    private readonly ILogger<NotificationApiController> _logger;
    private readonly NotificationQueries _notificationQueries;

    public NotificationApiController(ILogger<NotificationApiController> logger, NotificationQueries notificationQueries)
    {
        _logger = logger;
        _notificationQueries = notificationQueries;
    }
    [HttpGet("")]
    public async Task<IActionResult> GetNotifications(int pageNumber = 1, int pageSize = 7)
    {
        var currentUserId = HttpContext.GetUserId();
        if (currentUserId is null)
        {
            return Redirect("/Error/UnauthorizedPage");
        }
        var result = await _notificationQueries.GetNotificationPaginationAsync(currentUserId, pageNumber, pageSize);
        return Ok(result);
    }
}