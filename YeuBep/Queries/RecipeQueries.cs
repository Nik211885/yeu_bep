using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.Extensions;
using YeuBep.ViewModels;
using YeuBep.ViewModels.Comment;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.Queries;

public class RecipeQueries
{
    private readonly ILogger<RecipeQueries> _logger;
    private readonly YeuBepDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RecipeQueries(ILogger<RecipeQueries> logger, YeuBepDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<Result<PaginationViewModel<RecipeViewModel>>> GetMyRecipePaginationAsync(string? userId,
        int pageNumber, int pageSize)
    {
        var recipe = await _dbContext.Recipes.AsNoTracking()
            .Where(x=>x.CreatedById == userId)
            .OrderByDescending(r=>r.CreatedDate)
            .ProjectToType<RecipeViewModel>()
            .GetPaginationAsync(pageNumber, pageSize);
        return Result.Ok(recipe);
    }
    // not each user
    public async Task<Result<RecipeViewModel>> GetMyRecipeByIdAsync(string recipeId)
    {
        var recipe = await _dbContext.Recipes.AsNoTracking().ProjectToType<RecipeViewModel>()
            .Where(x => x.Id == recipeId)
            .FirstOrDefaultAsync();
        var currentUserId = _httpContextAccessor.HttpContext?.GetUserId();
        if (currentUserId == null || currentUserId != recipe?.CreatedBy.Id)
        {
            return Result.Fail("Bạn không có quyền truy cập tài nguyên này");
        }
        return Result.Ok(recipe);
    }

    public async Task<Result<PaginationViewModel<RecipeViewModel>>> GetTopRecipePaginationAsync(int pageNumber, int pageSize)
    {
        var recipe = await _dbContext.Recipes
            .Where(r=>r.RecipeStatus == RecipeStatus.Accept)
            .OrderByDescending(r=>r.TotalRatingPoint)
            .ThenByDescending(r=>r.CountRatingPoint)
            .ThenByDescending(r=>r.CountFavorite)
            .ThenByDescending(r=>r.CreatedDate)
            .ProjectToType<RecipeViewModel>()
            .AsNoTracking()
            .GetPaginationAsync(pageNumber, pageSize);
        return Result.Ok(recipe);
    }

    public async Task<Result<RecipeViewModel>> GetRecipeBySlugAsync(string slug)
    {
        var recipe = await _dbContext.Recipes
            .Where(x => x.Slug == slug)
            .Where(x => x.RecipeStatus == RecipeStatus.Accept)
            .ProjectToType<RecipeViewModel>()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (recipe == null)
        {
            return Result.Fail("Không tìm thấy công thức!");
        }
        recipe.Comments = recipe.Comments
            .OrderByDescending(c => c.CreatedDate)
            .ToList();
        return Result.Ok(recipe);
    }

    public async Task<Result<PaginationViewModel<RecipeViewModel>>> GetManagerRecipePaginationAsync(int pageNumber,
        int pageSize)
    {
        var recipe = await _dbContext.Recipes
            .Where(x => x.RecipeStatus != RecipeStatus.Draft)
            .Select(x => new
            {
                Recipe = x,
                OrderStatus = x.RecipeStatus == RecipeStatus.Send ? 1 :
                    x.RecipeStatus == RecipeStatus.Reject ? 2 :
                    x.RecipeStatus == RecipeStatus.Accept ? 3 : 4
            })
            .OrderBy(x => x.OrderStatus)
            .ThenByDescending(x => x.Recipe.CreatedDate)
            .Select(x => x.Recipe)
            .ProjectToType<RecipeViewModel>()
            .AsNoTracking()
            .GetPaginationAsync(pageNumber, pageSize);

        return Result.Ok(recipe);
    }
}