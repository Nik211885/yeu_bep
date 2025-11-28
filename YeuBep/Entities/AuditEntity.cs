namespace YeuBep.Entities;

public class AuditEntity
{
    public string Id { get;private set; } = Guid.CreateVersion7().ToString();
    public string CreatedById { get; set; }
    public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;
    public string? ModifiedById { get;  set; }
    public DateTimeOffset? ModifiedDate { get; set; }
    // navigation for ef
    public virtual User?  CreatedBy { get; set; }
    public virtual User?  ModifiedBy { get; set; }
}