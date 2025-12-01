using System.Text.Json.Serialization;
using YeuBep.Entities;
using YeuBep.Helpers;
using YeuBep.ViewModels.Account;

namespace YeuBep.ViewModels.Notification;

public class NotificationViewModel
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string Link { get; set; }
    public NotificationSubject NotificationSubject { get; set; }
    public AccountInfo SendForUser { get; set; }
    public AccountInfo CreatedBy { get; set; }
    [JsonIgnore]
    public DateTimeOffset CreatedDate { get; set; }
    public string Time => DateTimeHelper.ConvertDateTimeZoneFormat(CreatedDate);
}