using Microsoft.AspNetCore.Mvc;
using YeuBep.Services;
using YeuBep.ViewModels.Comment;

namespace YeuBep.Controllers.Apis;

[ApiController]
[Route("api/comments")]
public class CommentApiController : ControllerBase
{
    private readonly ILogger<FavoriteApiController> _logger;
    private readonly CommentServices _commentServices;

    public CommentApiController(ILogger<FavoriteApiController> logger, CommentServices commentServices)
    {
        _logger = logger;
        _commentServices = commentServices;
    }
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateCommentViewModel createCommentRequest)
    {
        var commentResponse = await _commentServices.CreateComment(createCommentRequest);
        if (commentResponse.IsFailed)
        {
            return BadRequest(commentResponse.Errors);
        }
        return Ok(commentResponse.Value);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string commentId)
    {
        var deleteCommentResult = await _commentServices.DeleteComment(commentId);
        if (deleteCommentResult.IsFailed)
        {
            return BadRequest(deleteCommentResult.Errors);
        }
        return NoContent();
    }
}