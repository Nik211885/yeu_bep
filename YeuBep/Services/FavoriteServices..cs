using FluentResults;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Entities;

namespace YeuBep.Services;

public class FavoriteServices
{
    private readonly YeuBepDbContext _dbContext;
    private readonly ILogger<FavoriteServices> _logger;

    public FavoriteServices(YeuBepDbContext dbContext, ILogger<FavoriteServices> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> ToggleFavoriteAsync(string recipeId, string createdBy)
    {
        var recipe = await _dbContext.Recipes.Where(x=>x.Id == recipeId)
            .FirstOrDefaultAsync();
        if (recipe is null)
        {
            return Result.Fail("Không tìm thấy công thức");
        }
        var favoriteRecipeExits = await _dbContext.Favorites
            .Where(x => x.RecipeId == recipeId && x.CreatedById == createdBy)
            .FirstOrDefaultAsync();
        if (favoriteRecipeExits == null)
        {
            var favorite = new Favorite()
            {
                RecipeId = recipeId
            };
            _dbContext.Favorites.Add(favorite);
            recipe.CountFavorite += 1;
        }
        else
        {
            recipe.CountFavorite -= 1;
            _dbContext.Favorites.Remove(favoriteRecipeExits);
        }
        _dbContext.Recipes.Update(recipe);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}