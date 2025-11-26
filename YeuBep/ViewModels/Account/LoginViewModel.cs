using System.ComponentModel.DataAnnotations;

namespace YeuBep.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Username hoặc Email là bắt buộc")]
    public string UserNameOrEmail { get; set; }
        
    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
        
    public bool RememberMe { get; set; }
}