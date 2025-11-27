using System.ComponentModel.DataAnnotations;
using YeuBep.Entities;

namespace YeuBep.ViewModels.Recipe;

public class CreateRecipeViewModel
{
    [Required(ErrorMessage = "Tên công thức bắt buộc!")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Mô tả công thức bắt buộc!")]
    public string Description { get; set; }
    [Required(ErrorMessage ="Khẩu phần ăn bắt buộc!")]
    public string PortionCount { get; set; }
    [Required(ErrorMessage = "Thời gian nấu bắt buộc")]
    public string TimeToCook { get; set; }
    [Required(ErrorMessage = "Ảnh mô tả bắt buộc!")]
    public string Avatar { get; set; }
    [Required(ErrorMessage = "Nguyên liệu bắt buộc")]
    public ICollection<IngredientPart> IngredientPart { get; set; }
    [Required(ErrorMessage = "Các bước bắt buộc")]
    public ICollection<DetailInstructionStep> DetailInstructionSteps { get; set; }
}