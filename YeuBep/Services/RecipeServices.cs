using FluentResults;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.Extensions;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.Services;

public class RecipeServices
{
    private readonly YeuBepDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RecipeServices> _logger;

    public RecipeServices(YeuBepDbContext dbContext, ILogger<RecipeServices> logger, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<RecipeViewModel>> CreateRecipeAsync(CreateRecipeViewModel model)
    {
        var recipe = model.Adapt<Recipe>();
        recipe.RecipeStatus = RecipeStatus.Draft;
        _dbContext.Recipes.Add(recipe);
        recipe.Slug = model.Title.GeneratorSlug();
        await _dbContext.SaveChangesAsync();
        return recipe.Adapt<RecipeViewModel>();
    }

    public async Task<Result<RecipeViewModel>> UpdateRecipeAsync(string recipeId, CreateRecipeViewModel model)
    {
        var recipe = await _dbContext.Recipes.Where(x => x.Id == recipeId).FirstOrDefaultAsync();
        if (recipe is null)
        {
            return Result.Fail("Không tìm thấy công thức để cập nhật");
        }

        if (!(_httpContextAccessor.HttpContext?.CheckPermission(recipe) ?? false))
        {
            return Result.Fail("Bạn không có quyền truy cập dữ liệu này");
        }
        if (recipe.RecipeStatus != RecipeStatus.Draft)
        {
            return Result.Fail("Công thức không thể cập nhật khi không trạng thái nháp");
        }
        model.Adapt(recipe);
        if (model.Title != recipe.Title)
        {
            recipe.Title = model.Title.GeneratorSlug();
        }
        _dbContext.Recipes.Update(recipe);
        await _dbContext.SaveChangesAsync();
        return recipe.Adapt<RecipeViewModel>();
    }

    public async Task<Result<RecipeViewModel>> UnpublishRecipeAsync(string recipeId)
    {
        var recipe = await _dbContext.Recipes.Where(x => x.Id == recipeId).FirstOrDefaultAsync();
        if (recipe is null)
        {
            return Result.Fail("Không tìm thấy công thức để cập nhật");
        }
        if (!(_httpContextAccessor.HttpContext?.CheckPermission(recipe) ?? false))
        {
            return Result.Fail("Bạn không có quyền truy cập dữ liệu này");
        }
        if (recipe.RecipeStatus != RecipeStatus.Draft)
        {
            return Result.Fail("Công thức không được duyệt không cần gỡ");
        }

        recipe.RecipeStatus = RecipeStatus.Draft;
        _dbContext.Recipes.Update(recipe);
        await _dbContext.SaveChangesAsync();
        return recipe.Adapt<RecipeViewModel>();
    }

    public async Task<Result> DeleteRecipeAsync(string recipeId)
    {
        var recipe = await _dbContext.Recipes.Where(x => x.Id == recipeId).FirstOrDefaultAsync();
        if (recipe is null)
        {
            return Result.Fail("Không tìm thấy công thức để cập nhật");
        }
        if (!(_httpContextAccessor.HttpContext?.CheckPermission(recipe) ?? false))
        {
            return Result.Fail("Bạn không có quyền truy cập dữ liệu này");
        }
        _dbContext.Recipes.Remove(recipe);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
    public async Task<Result<RecipeViewModel>> SendApproveRecipeAsync([FromQuery]string? recipeId,[FromBody] CreateRecipeViewModel? model)
    {
        Recipe? recipe;
        if (string.IsNullOrWhiteSpace(recipeId) && model is not null)
        {
            recipe = model.Adapt<Recipe>();
            recipe.Slug = model.Title.GeneratorSlug();
            recipe.RecipeStatus = RecipeStatus.Send;
            _dbContext.Recipes.Add(recipe);
            // create new instance
        }
        else
        {
            recipe = await _dbContext.Recipes.Where(x => x.Id == recipeId).FirstOrDefaultAsync();
            if (recipe is null)
            {
                return Result.Fail("Không tìm thấy công thức để gửi duyệt");
            }
            if (!(_httpContextAccessor.HttpContext?.CheckPermission(recipe) ?? false))
            {
                return Result.Fail("Bạn không có quyền truy cập dữ liệu này");
            }
            // nếu như bài đã duyệt hặc tư chối có thể gửi lại
            if (recipe.RecipeStatus == RecipeStatus.Send)
            {
                return Result.Fail("Công thức đang chờ kiểm duyệt!");
            }
            recipe.RecipeStatus = RecipeStatus.Send;
            model.Adapt(recipe);
            if (recipe.Title != model?.Title && model?.Title != null)
            {
                recipe.Slug = model.Title.GeneratorSlug();
            }
            _dbContext.Recipes.Update(recipe);
            // update instance
        }
        await _dbContext.SaveChangesAsync();
        return recipe.Adapt<RecipeViewModel>();
    }
}