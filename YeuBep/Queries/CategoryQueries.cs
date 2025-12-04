using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YeuBep.Controllers;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.ViewModels.Account;

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
}