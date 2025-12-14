using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YeuBep.Entities;
using YeuBep.Extensions;
using YeuBep.Queries;
using YeuBep.Services;

namespace YeuBep.Controllers.Apis;

[ApiController]
[Route("api/favorites")]
public class FavoriteApiController : ControllerBase
{
    private readonly ILogger<FavoriteApiController> _logger;
    private readonly FavoriteServices _favoriteServices;
    private readonly FavoritesQueries _favoritesQueries;

    public FavoriteApiController(ILogger<FavoriteApiController> logger, FavoriteServices favoriteServices, FavoritesQueries favoritesQueries)
    {
        _logger = logger;
        _favoriteServices = favoriteServices;
        _favoritesQueries = favoritesQueries;
    }

    [HttpPost("toggle")]
    public async Task<IActionResult> Toggle(string recipeId)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var favoriteResult = await _favoriteServices.ToggleFavoriteAsync(recipeId, userId);
        if (favoriteResult.IsFailed)
        {
            return BadRequest(favoriteResult.Errors);
        }
        return NoContent();
    }

    [HttpGet("my-favorite")]
    public async Task<IActionResult> MyFavoriteRecipe(string recipeId)
    {
        var userId = HttpContext.GetUserId();
        if (userId is null)
        {
            return NotFound();
        }
        var myFavoriteRecipe = await _favoritesQueries.MyFavoriteRecipeAsync(recipeId,userId);
        if (myFavoriteRecipe.IsFailed)
        {
            return NotFound();
        }

        return NoContent();
    }
}