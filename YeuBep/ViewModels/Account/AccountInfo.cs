using YeuBep.Attributes.Table;

namespace YeuBep.ViewModels.Account;

public class AccountInfo
{
    [IgnoreColumn]
    public Guid UserId { get; set; }
    [NameColumn("Người tạo")]
    public string UserName { get; set; }
    [IgnoreColumn]
    public string Bio { get; set; }
    [IgnoreColumn]
    public string Avatar { get; set; }
}