using YeuBep.Entities;
using YeuBep.Helpers;

namespace YeuBep.ViewModels.Notification;

public class CreateNotificationViewModel
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string Link { get; set; }
    public string SendForUserId { get; set; }
    public NotificationSubject NotificationSubject { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string Time => DateTimeHelper.ConvertDateTimeZoneFormat(CreatedDate);
}