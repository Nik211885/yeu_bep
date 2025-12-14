using System.ComponentModel.DataAnnotations;

namespace YeuBep.ViewModels.Category;

public class CreateCategoryViewModel
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Mô tả không được để trống")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Mô tả không được để trống")]
    public string Avatar { get; set; }
    public string? Emoji { get; set; }
}