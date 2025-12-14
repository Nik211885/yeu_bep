using YeuBep.Attributes.Table;
using YeuBep.ViewModels.Account;

namespace YeuBep.ViewModels.Category;

public class CategoryViewModel
{
    [KeyTable]
    [NameColumn("Id")]
    public string Id { get; set; }
    [NameColumn(("Tiêu đề"))]
    public string Title { get; set; }
    [IgnoreColumn]
    public string Slug { get; set; }
    [NameColumn("Mô tả")]
    public string Description { get; set; }
    [IgnoreColumn]
    public string Avatar { get; set; }
    [NameColumn("Ngày tạo")]
    public DateTimeOffset CreatedDate { get; set; }
    [NameColumn("Trạng thái")]
    public bool IsActive { get; set; }
    [IgnoreColumn]
    public AccountInfo CreatedBy { get; set; }
    [IgnoreColumn]
    public int CountRecipe { get; set; }
    [IgnoreColumn]
    public string Emoji { get; set; }
}