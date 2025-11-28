using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.Extensions;
using YeuBep.ViewModels;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.Queries;

public class FavoritesQueries
{
    private readonly ILogger<FavoritesQueries> _logger;
    private readonly YeuBepDbContext  _dbContext;

    public FavoritesQueries(ILogger<FavoritesQueries> logger, YeuBepDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Result<PaginationViewModel<RecipeViewModel>>> GetMyFavoritesRecipePaginationAsync(string userId, int pageNumber,
        int pageSize)
    {
        var favorites = await _dbContext.Recipes
            .Where(r => r.RecipeStatus == RecipeStatus.Accept)
            .Where(r => r.Favorites.Any(f => f.CreatedById == userId))
            .OrderByDescending(r => r.TotalRatingPoint)
            .ThenByDescending(r => r.CountRatingPoint)
            .ThenByDescending(r => r.CountFavorite)
            .ThenByDescending(r => r.CreatedDate)
            .ProjectToType<RecipeViewModel>()
            .AsNoTracking()
            .GetPaginationAsync(pageNumber, pageSize);
        return Result.Ok(favorites);
    }
    public async Task<Result> MyFavoriteRecipeAsync(string recipeId, string createdBy)
    {
        var favorite = await _dbContext.Favorites
            .Where(x => x.CreatedById == createdBy)
            .Where(x => x.RecipeId == recipeId)
            .FirstOrDefaultAsync();
        if (favorite is null)
        {
            return Result.Fail("");
        }
        return Result.Ok();
    }
}