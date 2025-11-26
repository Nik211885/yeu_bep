using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YeuBep.Entities;
using YeuBep.Extensions;
using YeuBep.Services;

namespace YeuBep.Controllers.Apis;

[ApiController]
[Route("api/favorites")]
public class FavoriteApiController : ControllerBase
{
    private readonly ILogger<FavoriteApiController> _logger;
    private readonly FavoriteServices _favoriteServices;

    public FavoriteApiController(ILogger<FavoriteApiController> logger, FavoriteServices favoriteServices)
    {
        _logger = logger;
        _favoriteServices = favoriteServices;
    }

    [HttpPost("toggle")]
    public async Task<IActionResult> Toggle(string recipeId)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var favoriteResult = await _favoriteServices.ToggleFavorite(recipeId, userId);
        if (favoriteResult.IsFailed)
        {
            return BadRequest(favoriteResult.Errors);
        }
        return NoContent();
    }
}