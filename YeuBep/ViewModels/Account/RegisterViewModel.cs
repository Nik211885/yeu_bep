using System.ComponentModel.DataAnnotations;

namespace YeuBep.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Username là bắt buộc")]
    public string UserName { get; set; }
        
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; }
        
    [Required(ErrorMessage = "Password là bắt buộc")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password phải ít nhất 6 ký tự")]
    public string Password { get; set; }
        
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Mật khẩu xác nhận là bắt buộc")]
    [Compare("Password", ErrorMessage = "Password không khớp")]
    public string ConfirmPassword { get; set; }
        
    public string FullName { get; set; }
}