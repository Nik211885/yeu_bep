using FluentResults;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.ViewModels.Comment;

namespace YeuBep.Services;

public class CommentServices 
{
    private readonly YeuBepDbContext _dbContext;
    private readonly ILogger<CommentServices> _logger;

    public CommentServices(YeuBepDbContext dbContext, ILogger<CommentServices> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<CommentViewModel>> CreateComment(CreateCommentViewModel commentRequest)
    {
        var recipe = await _dbContext.Recipes.Where(x=>x.Id == commentRequest.RecipeId)
            .FirstOrDefaultAsync();
        if (recipe is null)
        {
            return Result.Fail("Không tìm thấy công thức");
        }
        var commentCreate = new Comment()
        {
            RecipeId = commentRequest.RecipeId,
            CommentText = commentRequest.CommentText,
        };
        var commentEntry = _dbContext.Comments.Add(commentCreate);
        var comment = commentEntry.Entity;
        await _dbContext.SaveChangesAsync();
        return Result.Ok(comment.Adapt<CommentViewModel>());
    }

    public async Task<Result> DeleteComment(string commentId)
    {
        var commentExits = await _dbContext.Comments.Where(x => x.Id == commentId).FirstOrDefaultAsync();
        if (commentExits is null)
        {
            return Result.Fail("Không tìm thấy bình luận");
        }

        _dbContext.Comments.Remove(commentExits);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}