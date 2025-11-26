using System.ComponentModel.DataAnnotations;

namespace YeuBep.ViewModels.Account;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email là bắt buộc")]
    public string Email { get; set; }
}