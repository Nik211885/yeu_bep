using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.ViewModels.Rating;

namespace YeuBep.Queries;

public class RatingQueries
{
    private readonly ILogger<RatingQueries> _logger;
    private readonly YeuBepDbContext _dbContext;

    public RatingQueries(ILogger<RatingQueries> logger, YeuBepDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Result<RatingViewModel>> GetMyRatingAsync(string userId, string recipeId)
    {
        var recipe = await _dbContext.Ratings
            .Where(x => x.CreatedById == userId)
            .Where(x => x.RecipeId == recipeId)
            .ProjectToType<RatingViewModel>()
            .FirstOrDefaultAsync();
        if (recipe == null)
        {
            return Result.Fail("");
        }
        return Result.Ok(recipe);
    }
}