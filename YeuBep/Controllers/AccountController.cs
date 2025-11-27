using System.Security.Claims;
using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using YeuBep.Const;
using YeuBep.Entities;
using YeuBep.Extends;
using YeuBep.ViewModels.Account;

namespace YeuBep.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly EmailSenderServices _emailSenderServices;

    public AccountController(ILogger<AccountController> logger, EmailSenderServices emailSenderServices, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSenderServices = emailSenderServices;
    }
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (HttpContext.User.Identity?.IsAuthenticated == true)
        {
            return RedirectToLocal(returnUrl);
        }
        var context = HttpContext;
        ViewData["ReturnUrl"] = returnUrl;  
        return View("LoginPage");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.UserNameOrEmail)
                       ?? await _userManager.FindByEmailAsync(model.UserNameOrEmail);
                
            if (user is not null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName!,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true);
                    
                if (result.Succeeded)
                {
                    TempData["Success"] = "Đăng nhập thành công!";
                    return RedirectToLocal(returnUrl);
                }
                    
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty,"Tài khoản đã bị khóa do đăng nhập sai quá nhiều lần.");
                }
            }
            ModelState.AddModelError(string.Empty, "Username hoặc mật khẩu không đúng.");
        }
        var errors = string.Join("<br/>",ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList());
        TempData["Error"] = errors;
        return View("LoginPage", model);
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View("Register");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
            };
                
            var result = await _userManager.CreateAsync(user, model.Password);
                
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, code = encodedToken },
                    protocol: Request.Scheme
                );
            
                string mailTemplateConfirmEmail = Template.RegisterSuccessEmailSenderBody(user.FullName, callbackUrl);

                BackgroundJob.Enqueue<EmailSenderServices>(s =>
                    _emailSenderServices.SendEmailAsync(user.Email,
                    mailTemplateConfirmEmail,
                    Template.RegisterSuccessEmailSenderSubject,
                    user.FullName));
                
                string message = Template.RegisterSuccessNotification;
                ViewData["Message"] = message;
                ViewData["MessageType"] = "success";
                ViewData["ReturnUrl"] = @Url.Action("Index",  "Home");
                return RedirectToAction("Notification", "Statics");
            }
                
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
            
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return View("NotFoundError");
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
        if (result.Succeeded)
        {
            string message = Template.ConfirmEmailSuccess;
            ViewData["Message"] = message;
            ViewData["MessageType"] = "success";
            ViewData["ReturnUrl"] = @Url.Action("Index",  "Home");
            return RedirectToAction("Notification", "Statics");
        }
        else
        {
            return View("Error");
        }

    }
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View("ForgotPassword");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            return View("Error");
        }
        var codeForgotPassword = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codeForgotPassword));
        var callbackUrl = Url.Action(
            "ResetPassword",
            "Account",
            new { userId = user.Id, code = encodedToken },
            protocol: Request.Scheme
        );
        string mailTemplateForgotPassword = Template.ForgotPasswordEmailSenderBody(user.FullName, callbackUrl);
        BackgroundJob.Enqueue<EmailSenderServices>(s =>
            _emailSenderServices.SendEmailAsync(user.Email!,
                mailTemplateForgotPassword,
                Template.ForgotPasswordEmailSenderSubject,
                user.FullName));
        string message = Template.SendEmailForgetPasswordSuccess;
        ViewData["Message"] = message;
        ViewData["MessageType"] = "success";
        ViewData["ReturnUrl"] = @Url.Action("Index",  "Home");
        return RedirectToAction("Notification", "Statics");
    }

    [HttpGet]
    public IActionResult ResetPassword(string userId, string code)
    {
        var model = new ResetPasswordViewModel()
        {
            UserId = userId,
            Token = code,
        };
        return View("ResetPassword", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user is null)
        {
            return View("NotFoundError");
        }
        var decodeToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        if (user.PasswordHash is not null)
        {
            if (_userManager.PasswordHasher.VerifyHashedPassword(user,user.PasswordHash, model.Password) ==
                PasswordVerificationResult.Success)
            {
                TempData["Error"] = "Mật khẩu mới không được trùng với mật khẩu cũ";
                return View("ResetPassword");
            }
        }
        var resetPasswordResult = await _userManager.ResetPasswordAsync(user, decodeToken, model.Password);
        if (resetPasswordResult.Succeeded)
        {
            TempData["Success"] = "Thiết lập mật khẩu mới thành công";
            return View("LoginPage");
        }
        return View("ResetPassword");
    }
    
    [HttpPost]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Content("~/");

        if (remoteError != null)
        {
            TempData["Error"] = $"External provider error: {remoteError}";
            return RedirectToAction(nameof(Login));
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction(nameof(Login));
        
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var fullName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            var user = new User()
            {
                Email = email,
                FullName = fullName,
            };
            var createUserResult = await _userManager.CreateAsync(user);
            if (createUserResult.Succeeded)
            {
                var loginResult = await _userManager.AddLoginAsync(user, info);
                if (loginResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return LocalRedirect(returnUrl);
        }
    }

    [HttpGet]
    [Authorize]
    public IActionResult Update()
    {
        return View("UpdateAccount");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateUserViewModel model)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View("ChangePassword");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> ChangPassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return View("NotFoundError");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return View("NotFoundError");
            }

            var validationPasswordResult = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if (!validationPasswordResult)
            {
                TempData["Error"] = "Mật khẩu cũ không đúng";
                return View("ChangePassword");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                TempData["Success"] = "Thay đổi mật khẩu thành công";
                return RedirectToAction("Index", "Home");
            }
        }
        return View("ChangePassword", model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync(); 

        return RedirectToAction("Index", "Home"); 
    }
    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }
}