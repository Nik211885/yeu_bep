using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YeuBep.Entities;
using YeuBep.Extensions;
using YeuBep.ViewModels;
using YeuBep.ViewModels.Account;

namespace YeuBep.Controllers.Manager;
[Authorize(Roles = nameof(Role.Admin))]
public class AccountManagerController : Controller
{
    private readonly ILogger<RecipeManagerController> _logger;
    private readonly UserManager<User> _userManager;

    public AccountManagerController(ILogger<RecipeManagerController> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }
    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 5)
    {
        var user = await _userManager.Users
            .ProjectToType<UserViewModel>()
            .GetPaginationAsync(pageNumber, pageSize);
        return View("~/Views/Manager/Account.cshtml", user.CastToObjectType());
    }
}