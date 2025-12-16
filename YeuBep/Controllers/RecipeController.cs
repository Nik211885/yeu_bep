using Hangfire;
using YeuBep.Data;
using YeuBep.Extensions;
using YeuBep.Queries;
using YeuBep.Services;
using YeuBep.ViewModels;
namespace YeuBep.Controllers;
using Microsoft.AspNetCore.Mvc;

public class RecipeController : Controller
{
    private readonly  ILogger<RecipeController> _logger;
    private readonly RecipeQueries _recipeQueries;
    private readonly YeuBepDbContext _dbContext;
    private readonly RecipeServices _recipeServices;
    
    public RecipeController(ILogger<RecipeController> logger, YeuBepDbContext dbContext, RecipeServices recipeServices, RecipeQueries recipeQueries)
    {
        _logger = logger;
        _dbContext = dbContext;
        _recipeServices = recipeServices;
        _recipeQueries = recipeQueries;
    }

    [HttpGet]
    public async Task<IActionResult> Slug(string slug)
    {
        var recipe = await _recipeQueries.GetRecipeBySlugAsync(slug);
        if (recipe.IsFailed)
        {
            return Redirect("/Error/NotFoundPage");
        }

        BackgroundJob.Enqueue<RecipeServices>(s => s.IncreaseViewAsync(recipe.Value.Id));
        return View("Recipe", recipe.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Recipe(string? search, int pageNumber = 1 , int pageSize = 5)
    {
        var result = await _recipeQueries.GetRecipePaginationBySearchTermAsync(search, pageNumber, pageSize);
        return View("RecipeList", result);
    }

    [HttpGet]
    public async Task<IActionResult> MyRecipe(int pageNumber = 1, int pageSize = 4, [AsParameters] Dictionary<string, string>? filterEqualTable = null)
    {
        var userId = HttpContext.GetUserId();
        if (userId is null)
        {
            return Redirect("/Error/UnauthorizedPage");
        }
        var recipe = await _recipeQueries.GetMyRecipePaginationAsync(userId, pageNumber, pageSize, filterEqualTable);
        return View("MyRecipeList", recipe.Value.CastToObjectType());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("CreateRecipe");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string recipeId)
    {
        var result = await _recipeQueries.GetMyRecipeByIdAsync(recipeId);
        if (result.IsFailed)
        {
            return Redirect("/Error/NotFoundPage");
        }
        return View("CreateRecipe", result.Value);
    }
}