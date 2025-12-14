using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YeuBep.Controllers;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.Extensions;
using YeuBep.ViewModels;
using YeuBep.ViewModels.Account;
using YeuBep.ViewModels.Category;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.Queries;

public class CategoryQueries
{
    private readonly ILogger<CategoryController> _logger;
    private readonly YeuBepDbContext _yeuBepDbContext;
    private readonly UserManager<User> _userManager;
    public CategoryQueries(ILogger<CategoryController> logger, YeuBepDbContext yeuBepDbContext, UserManager<User> userManager)
    {
        _logger = logger;
        _yeuBepDbContext = yeuBepDbContext;
        _userManager = userManager;
    }

    public async Task<List<UserViewModel>> GetTopChefAsync(int top)
    {
        var topUserId = await _yeuBepDbContext.Recipes
            .Where(x => x.RecipeStatus == RecipeStatus.Accept)
            .Where(x => x.CreatedDate.Month == DateTime.Today.Month)
            .GroupBy(x => x.CreatedById)
            .Select(g => new
            {
                UserId = g.Key,
                TotalFavorites = g.Sum(x => x.CountFavorite),
                CountRating = g.Sum(x => x.CountRatingPoint),
                TotalRatingPoint = g.Sum(x => x.TotalRatingPoint)
            })
            .OrderByDescending(x => x.CountRating)
            .ThenByDescending(x => x.TotalFavorites)
            .ThenByDescending(x => x.TotalRatingPoint)
            .Select(x => x.UserId)
            .Take(top)
            .ToListAsync();
        var chefs = await _userManager.Users
            .Where(x => topUserId.Contains(x.Id))
            .ProjectToType<UserViewModel>()
            .ToListAsync();
        return chefs;
    }

    public async Task<List<CategoryViewModel>> GetCategoriesAsync()
    {
        var categories = await _yeuBepDbContext.Categories
            .Where(x => x.IsActive)
            .OrderByDescending(x=>x.CountRecipe)
            .ThenByDescending(x=>x.CreatedDate)
            .ProjectToType<CategoryViewModel>()
            .ToListAsync();
        return categories;
    }

    public async Task<CategoryViewModel?> GetCategoryByIdAsync(string id)
    {
        var categories = await _yeuBepDbContext.Categories
            .Where(x=>x.Id == id)
            .ProjectToType<CategoryViewModel>()
            .FirstOrDefaultAsync();
        return categories;
    }

    public async Task<PaginationViewModel<CategoryViewModel>> GetCategoryPaginationAsync(int pageNumber, int pageSize,
        Dictionary<string, string>? filterEqualTableViewModel)
    {
        var categories = await _yeuBepDbContext.Categories.AsNoTracking()
            .OrderByDescending(r=>r.CreatedDate)
            .ProjectToType<CategoryViewModel>()
            .WhereEqualFilterValue(filterEqualTableViewModel)
            .GetPaginationAsync(pageNumber, pageSize);
        return categories;
    }

    public async Task<CategoryViewModel?> GetCategoryBySlugAsync(string slug)
    {
        var categoryBySlug = await _yeuBepDbContext.Categories.AsNoTracking()
            .Where(x => x.Slug == slug)
            .Where(x=>x.IsActive)
            .ProjectToType<CategoryViewModel>()
            .FirstOrDefaultAsync();
        return categoryBySlug;
    }

    public async Task<List<CategoryViewModel>> GetCategoryByRecipeAsync(string recipeId)
    {
        var categories = await _yeuBepDbContext.Categories
            .Where(x => x.IsActive)
            .Where(x => x.CategoriesRecipes
                .Any(cr => cr.RecipeId == recipeId))
            .ProjectToType<CategoryViewModel>()
            .AsNoTracking()
            .ToListAsync();
        return categories;
    }

    public async Task<List<RecipeViewModel>> GetRecipeMostViewAsync(int top)
    {
        var recipes = await _yeuBepDbContext.Recipes
            .Where(x => x.RecipeStatus == RecipeStatus.Accept)
            .OrderByDescending(x => x.Views)
            .ThenByDescending(x => x.CreatedBy)
            .Take(top)
            .ProjectToType<RecipeViewModel>()
            .AsNoTracking()
            .ToListAsync();
        foreach (var recipe in recipes)
        {
            var categoriesId = await _yeuBepDbContext.CategoriesRecipes
                .Where(x=>x.RecipeId == recipe.Id)
                .Select(x=>x.CategoryId)
                .ToListAsync();
            var categories = await _yeuBepDbContext.Categories
                .Where(x => categoriesId.Contains(x.Id))
                .Where(x=>x.IsActive)
                .ProjectToType<CategoryViewModel>()
                .ToListAsync();
            recipe.CategoriesCollection = categories;
        }
        return recipes;
    }
}