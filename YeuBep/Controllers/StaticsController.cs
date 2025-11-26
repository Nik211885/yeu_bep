using Microsoft.AspNetCore.Mvc;
using YeuBep.Const;

namespace YeuBep.Controllers;

public class StaticsController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public StaticsController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Notification()
    {
        string message = Template.RegisterSuccessNotification;
        ViewData["Message"] = message;
        ViewData["MessageType"] = "success";
        ViewData["ReturnUrl"] = @Url.Action("Index",  "Home");
        return View("Notification");
    }
}