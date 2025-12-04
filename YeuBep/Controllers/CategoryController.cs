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

    public CategoryController(ILogger<CategoryController> logger, CategoryQueries categoryQueries, YeuBepDbContext dbContext)
    {
        _logger = logger;
        _categoryQueries = categoryQueries;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        return View("~/Views/Categorie/Category.cshtml");
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
            return Redirect($"Error/NotFoundPage");
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
}