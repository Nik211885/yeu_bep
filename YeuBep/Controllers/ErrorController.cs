using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using YeuBep.ViewModels.Errors;

namespace YeuBep.Controllers;

public class ErrorController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult NotFoundPage()
    {
        return View("NotFoundError", new NotFoundViewModel() { RequestedPath = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult UnauthorizedPage()
    {
        var model = new UnauthorizedViewModel
        {
            RequestedPath = HttpContext.Request.Path,
            Message = "Bạn không được phép truy cập trang này."
        };

        return View("Unauthorized", model);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ServerErrorPage()
    {
        var model = new InternalServerViewModel()
        {
            RequestedPath = HttpContext.Request.Path,
            Message = "Đã xảy ra lỗi máy chủ nội bộ, vui lòng thử lại sau.",
            ErrorId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        return View("ServerError", model);
    }
}