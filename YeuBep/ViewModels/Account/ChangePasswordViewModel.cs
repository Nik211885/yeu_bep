using System.ComponentModel.DataAnnotations;

namespace YeuBep.ViewModels.Account;

public class ChangePasswordViewModel
{
    public string OldPassword { get; set; }
    [Required(ErrorMessage = "Password là bắt buộc")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password phải ít nhất 6 ký tự")]
    public string NewPassword { get; set; }
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Mật khẩu xác nhận là bắt buộc")]
    [Compare("NewPassword", ErrorMessage = "Password không khớp")]
    public string ConfirmPassword { get; set; }
}