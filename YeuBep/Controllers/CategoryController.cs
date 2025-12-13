using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.Extensions;
using YeuBep.Queries;
using YeuBep.ViewModels.Account;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.Controllers;

public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly CategoryQueries _categoryQueries;
    private readonly YeuBepDbContext _dbContext;
    private readonly RecipeQueries _recipeQueries;

    public CategoryController(ILogger<CategoryController> logger, CategoryQueries categoryQueries, YeuBepDbContext dbContext, RecipeQueries recipeQueries)
    {
        _logger = logger;
        _categoryQueries = categoryQueries;
        _dbContext = dbContext;
        _recipeQueries = recipeQueries;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryQueries.GetCategoriesAsync();
        return View("~/Views/Categorie/Category.cshtml", categories);
    }

    [HttpGet]
    public async Task<IActionResult> GetTrending()
    {
        return View("~/Views/Categorie/Trending.cshtml");
    }

    [HttpGet]
    public async Task<IActionResult> GetChefs()
    {
        var chefs = await _categoryQueries.GetTopChefAsync(5);
        return View("~/Views/Categorie/Chef.cshtml", chefs);
    }

    [HttpGet]
    public async Task<IActionResult> GetChefById(string id, int pageNumber = 1, int pageSize = 5)
    {
        var chef = await _dbContext.Users.Where(x => x.Id == id)
            .ProjectToType<UserViewModel>()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (chef is null)
        {
            return RedirectToAction($"Error/NotFoundPage");
        }

        var recipeByChef = await _dbContext
            .Recipes.Where(x => x.RecipeStatus == RecipeStatus.Accept)
            .Where(x => x.CreatedById == chef.Id)
            .OrderByDescending(x => x.CreatedDate)
            .ProjectToType<RecipeViewModel>()
            .AsNoTracking()
            .GetPaginationAsync(pageNumber, pageSize);
        return View("~/Views/Categorie/ChefDetail.cshtml", (chef, recipeByChef));
    }

    [HttpGet]
    public async Task<IActionResult> Slug(string slug, int pageNumber =1, int pageSize = 5)
    {
        var category = await _categoryQueries.GetCategoryBySlugAsync(slug);
        if (category is null)
        {
            return RedirectToAction($"Error/NotFoundPage"); 
        }
        var recipes = await _dbContext
            .Recipes.Where(x => x.RecipeStatus == RecipeStatus.Accept)
            .Where(x => x.CategoriesRecipes.Any(y=>y.CategoryId == category.Id))
            .OrderByDescending(x => x.CreatedDate)
            .ProjectToType<RecipeViewModel>()
            .AsNoTracking()
            .GetPaginationAsync(pageNumber, pageSize);
        return View("~/Views/Categorie/CategoryBySlug.cshtml", (category, recipes));
    }
}