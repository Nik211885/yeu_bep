namespace YeuBep.Entities;

public class Notification : AuditEntity
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string Link { get; set; }
    public string SendForUserId { get; set; }
    public User SendForUser { get; set; }
    public NotificationSubject NotificationSubject { get; set; }
}