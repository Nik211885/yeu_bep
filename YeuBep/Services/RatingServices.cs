using FluentResults;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.ViewModels.Rating;

namespace YeuBep.Services;

public class RatingServices
{
    private readonly YeuBepDbContext _dbContext;
    private readonly ILogger<FavoriteServices> _logger;

    public RatingServices(YeuBepDbContext dbContext, ILogger<FavoriteServices> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> RatingAsync(CreateRatingViewModel ratingRequest, string userId)
    {
        var recipe = await _dbContext.Recipes.Where(x=>x.Id == ratingRequest.RecipeId)
            .FirstOrDefaultAsync();
        if (recipe is null)
        {
            return Result.Fail("Không tìm thấy công thức");
        }
        var ratingExit = await _dbContext.Ratings
            .Where(x => x.CreatedById == userId && x.RecipeId == ratingRequest.RecipeId)
            .FirstOrDefaultAsync();
        if (ratingExit is not null)
        {
            ratingExit.RatingPoint =  ratingRequest.RatingPoint;
            recipe.TotalRatingPoint -= ratingExit.RatingPoint;
        }
        else
        {
            recipe.CountRatingPoint += 1;
        }
        if (ratingRequest.RatingPoint is < 0 or > 5)
        {
            return Result.Fail("Giá trị đánh giá không hợp lệ");
        }
        var rating = new Rating()
        {
            RecipeId = ratingRequest.RecipeId,
            RatingPoint = ratingRequest.RatingPoint,
        };
        recipe.TotalRatingPoint += ratingRequest.RatingPoint;
        _dbContext.Recipes.Update(recipe);
        _dbContext.Ratings.Add(rating);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}