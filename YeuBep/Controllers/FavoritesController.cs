using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YeuBep.Extensions;
using YeuBep.Queries;
using YeuBep.ViewModels;

namespace YeuBep.Controllers;


public class FavoritesController : Controller
{
    private readonly ILogger<FavoritesController> _logger;
    private readonly FavoritesQueries _recipeQueries;

    public FavoritesController(ILogger<FavoritesController> logger, FavoritesQueries recipeQueries)
    {
        _logger = logger;
        _recipeQueries = recipeQueries;
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> MyFavorite(int pageNumber = 1, int pageSize = 5)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return Redirect("/Error/UnauthorizedPage");
        }
        var recipeFavorites = await _recipeQueries.GetMyFavoritesRecipePaginationAsync(userId, pageNumber, pageSize); 
        return View("MyFavorites", recipeFavorites.Value);
    }
}