using YeuBep.Attributes.Table;

namespace YeuBep.ViewModels.Account;

public class UserViewModel
{
    [KeyTable]
    [NameColumn("userId")]
    public string Id { get; set; }
    [NameColumn("Họ tên")]
    public string? FullName { get; set; } = string.Empty;
    [NameColumn("Username")]
    public string UserName { get; set; } = string.Empty;
    [NameColumn("Email")]
    public string Email { get; set; } = string.Empty;
    [IgnoreColumn]
    public bool EmailConfirmed { get; set; }
    [NameColumn("Xác nhận email")]
    public string EmailConfirmDisplay => EmailConfirmed ? "Đã xác nhận" : "Chưa xác nhận";
    [NameColumn("Số điện thoại")]
    public string? PhoneNumber { get; set; }
    [NameColumn("Thời gian khóa")]
    public DateTimeOffset? LockoutEnd { get; set; }
    [IgnoreColumn]
    public string Avatar{get;set;} = string.Empty;
    [IgnoreColumn]
    public string Bio { get; set; } = string.Empty;
    [IgnoreColumn]
    public string Address { get; set; } = string.Empty;
    [IgnoreColumn]
    public ICollection<string> Majors { get; set; } = [];
}