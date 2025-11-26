using Mapster;
using Microsoft.EntityFrameworkCore;
using YeuBep.Const;
using YeuBep.Data;
using YeuBep.Services;
using YeuBep.ViewModels;
using YeuBep.ViewModels.Comment;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.Controllers;
using Microsoft.AspNetCore.Mvc;

public class RecipeController : Controller
{
    private readonly  ILogger<RecipeController> _logger;
    private readonly YeuBepDbContext _dbContext;
    private readonly RecipeServices _recipeServices;
    
    public RecipeController(ILogger<RecipeController> logger, YeuBepDbContext dbContext, RecipeServices recipeServices)
    {
        _logger = logger;
        _dbContext = dbContext;
        _recipeServices = recipeServices;
    }

    [HttpGet("Recipe/{slug}")]
    public async Task<IActionResult> RecipeBySlug(string slug)
    {
        // if (string.IsNullOrWhiteSpace(slug))
        // {
        //     return RedirectToAction("NotFoundPage", "Error");
        // }
        // var recipe = await _dbContext.Recipes
        //     .Include(x=>x.CreatedBy)
        //     .ProjectToType<RecipeViewModel>()
        //     .AsNoTracking()
        //     .FirstOrDefaultAsync(x=>x.Slug == slug);
        // if (recipe == null)
        // {
        //     return RedirectToAction("NotFoundPage", "Error");
        // }
        //
        // var comments = await _dbContext.Comments
        //     .Include(x=>x.CreatedBy)
        //     .ProjectToType<CommentViewModel>()
        //     .AsNoTracking()
        //     .Where(x => x.RecipeId == recipe.Id).ToListAsync();
        //
        // recipe.Comments = comments;
        var recipe = FakeData.Recipe.FirstOrDefault(r => r.Slug == slug);
        return View("Recipe", recipe);
    }

    [HttpGet("Recipe")]
    public async Task<IActionResult> Recipe()
    {
        var recipe = FakeData.Recipe;
        var pagination = new PaginationViewModel<RecipeViewModel>(recipe, 1, 10, 30);
        return View("RecipeList", pagination);
    }

    [HttpGet("MyRecipe")]
    public async Task<IActionResult> MyRecipe()
    {
        var recipe = FakeData.Recipe;
        var pagination = new PaginationViewModel<object>(recipe, 1, 10, 30);
        return View("MyRecipeList", pagination);
    }

    [HttpGet("Create")]
    public IActionResult CreateRecipe()
    {
        return View("CreateRecipe");
    }
    
}