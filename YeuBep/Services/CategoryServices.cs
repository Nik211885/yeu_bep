using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.Extensions;
using YeuBep.ViewModels.Category;

namespace YeuBep.Services;

public class CategoryServices
{
    private readonly YeuBepDbContext _dbContext;
    private readonly ILogger<CategoryServices> _logger;

    public CategoryServices(YeuBepDbContext dbContext, ILogger<CategoryServices> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<CategoryViewModel>> CreateCategoryAsync(CreateCategoryViewModel model)
    {
        var category = model.Adapt<Category>();
        category.IsActive = true;
        category.Slug = model.Title.GeneratorSlug();
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
        return Result.Ok(category.Adapt<CategoryViewModel>());
    }

    public async Task<Result<CategoryViewModel>> UpdateCategoryAsync(string categoryId, CreateCategoryViewModel model)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(x=>x.Id == categoryId);
        if (category is null)
        {
            return Result.Fail("");
        }
        category = model.Adapt(category);
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        return Result.Ok(category.Adapt<CategoryViewModel>());
    }

    public async Task<Result> DeleteCategoryAsync(string categoryId)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(x=>x.Id == categoryId);
        if (category is null)
        {
            return Result.Fail("");
        }
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result<CategoryViewModel>> ToggleCategoryAsync(string categoryId)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(x=>x.Id == categoryId);
        if (category is null)
        {
            return Result.Fail("");
        }
        category.IsActive = !category.IsActive;
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        return Result.Ok(category.Adapt<CategoryViewModel>());
    }

    public async Task<Result> DeleteCategoriesByIdsAsync(List<string> ids)
    {
        await _dbContext.Categories.Where(x => ids.Contains(x.Id))
            .ExecuteDeleteAsync();
        return Result.Ok();
    }
}