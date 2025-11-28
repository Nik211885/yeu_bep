using YeuBep.Attributes.Table;

namespace YeuBep.ViewModels.Account;

public class AccountInfo
{
    [IgnoreColumn]
    public string Id { get; set; }
    [NameColumn("Người tạo")]
    public string UserName { get; set; }
    [IgnoreColumn]
    public string? Bio { get; set; }
    [IgnoreColumn]
    public string? Avatar { get; set; }
    [IgnoreColumn]
    public string? FullName { get; set; }
}