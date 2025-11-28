using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YeuBep.Entities;
using YeuBep.Queries;
using YeuBep.ViewModels;

namespace YeuBep.Controllers.Manager;

[Authorize(Roles = nameof(Role.Admin))]
public class RecipeManagerController : Controller
{
    private readonly ILogger<RecipeManagerController> _logger;
    private readonly RecipeQueries _recipeQueries;

    public RecipeManagerController(ILogger<RecipeManagerController> logger, RecipeQueries recipeQueries)
    {
        _logger = logger;
        _recipeQueries = recipeQueries;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 5)
    {
        var recipe = await _recipeQueries.GetManagerRecipePaginationAsync(pageNumber, pageSize);
        return View("~/Views/Manager/Recipe.cshtml", recipe.Value.CastToObjectType());
    }
}