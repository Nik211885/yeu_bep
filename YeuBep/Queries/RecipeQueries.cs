using Dapper;
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
        int pageNumber, int pageSize, Dictionary<string, string>? filterEqualTableViewModel)
    {
        var recipe = await _dbContext.Recipes.AsNoTracking()
            .Where(x=>x.CreatedById == userId)
            .OrderByDescending(r=>r.CreatedDate)
            .ProjectToType<RecipeViewModel>()
            .WhereEqualFilterValue(filterEqualTableViewModel)
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
    public async Task<RecipeViewModel?> GetManagerRecipeDetailAsync(string recipeId)
    {
        var recipe = await _dbContext.Recipes
            .Where(x => x.RecipeStatus != RecipeStatus.Draft)
            .Where(x=>x.Id == recipeId)
            .ProjectToType<RecipeViewModel>()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return recipe;
    }

    public async Task<List<string>> SearchIntegrationAsync(string integration)
    {
        var con = _dbContext.Database.GetDbConnection();
        var fillText = await con.QueryAsync<string>(@"
                    SELECT DISTINCT ing AS ingredient
                    FROM ""Recipes"",
                         LATERAL jsonb_array_elements(""IngredientPart"") part,
                         LATERAL jsonb_array_elements_text(part->'Ingredients') ing
                    WHERE (unaccent(ing) ILIKE unaccent(@Keyword) OR ing ILIKE @Keyword)
                      AND ""RecipeStatus"" = 'Accept'
                    ORDER BY ing
                    LIMIT 5;", 
            new { 
                Keyword = $"%{integration}%" 
            });
        return fillText.ToList();
    }

    public async Task<PaginationViewModel<RecipeViewModel>> GetRecipePaginationBySearchTermAsync(string? searchTerm,
        int pageNumber, int pageSize)
    {
        searchTerm = (searchTerm ?? "").Trim();
        var keyword = $"%{searchTerm}%";

        pageNumber = Math.Max(pageNumber, 1);
        pageSize = pageSize <= 0 ? 10 : pageSize;
        var offset = (pageNumber - 1) * pageSize;
        
        var items = await _dbContext.Recipes
            .FromSqlInterpolated($@"
                    SELECT r.*
                    FROM ""Recipes"" r
                    WHERE r.""RecipeStatus"" = 'Accept'
                      AND EXISTS (
                        SELECT 1
                        FROM jsonb_array_elements(r.""IngredientPart"") AS part
                        CROSS JOIN jsonb_array_elements_text(part->'Ingredients') AS ing
                        WHERE unaccent(ing) ILIKE unaccent({keyword})
                           OR ing ILIKE {keyword}
                      )
                    ORDER BY r.""CreatedDate"" DESC
                    LIMIT {pageSize} OFFSET {offset}")
            .AsNoTracking()
            .ProjectToType<RecipeViewModel>()
            .ToListAsync();
        
        var totalCount = await _dbContext.Recipes
            .FromSqlInterpolated($@"
                    SELECT r.*
                    FROM ""Recipes"" r
                    WHERE r.""RecipeStatus"" = 'Accept'
                      AND EXISTS (
                        SELECT 1
                        FROM jsonb_array_elements(r.""IngredientPart"") AS part
                        CROSS JOIN jsonb_array_elements_text(part->'Ingredients') AS ing
                        WHERE unaccent(ing) ILIKE unaccent({keyword})
                           OR ing ILIKE {keyword}
                      )")
            .CountAsync();

        return new PaginationViewModel<RecipeViewModel>(
            items: items,
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalCount: totalCount
        );
    }
}