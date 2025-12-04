using System.ComponentModel.DataAnnotations;

namespace YeuBep.ViewModels.Account;

public class UpdateUserViewModel
{
    [Required(ErrorMessage = "Ảnh đại diện bắt buộc!")]
    public string Avatar { get; set; }
    [Required(ErrorMessage = "Giới thiệu bạn thân bắt buộc!")]
    public string Bio { get; set; }
    [Required(ErrorMessage = "Tên bắt buộc!")]
    public string FullName { get; set; }
    public string Address { get; set; } = string.Empty;
    public ICollection<string> Majors { get; set; } = [];
}