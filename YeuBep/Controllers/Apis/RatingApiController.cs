using Microsoft.AspNetCore.Mvc;
using YeuBep.Extensions;
using YeuBep.Queries;
using YeuBep.Services;
using YeuBep.ViewModels.Rating;

namespace YeuBep.Controllers.Apis;

[ApiController]
[Route("api/rating")]
public class RatingApiController : ControllerBase
{
    private readonly RatingServices _ratingServices;
    private readonly ILogger<RatingApiController> _logger;
    private readonly RatingQueries _ratingQueries;

    public RatingApiController(RatingServices ratingServices, ILogger<RatingApiController> logger, RatingQueries ratingQueries)
    {
        _ratingServices = ratingServices;
        _logger = logger;
        _ratingQueries = ratingQueries;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Rating(CreateRatingViewModel ratingRequest)
    {
        var userId = HttpContext.GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var ratingResult = await _ratingServices.RatingAsync(ratingRequest, userId);
        if (ratingResult.IsFailed)
        {
            return BadRequest(ratingResult.Errors);
        }

        return NoContent();
    }

    [HttpGet("my-rating")]
    public async Task<IActionResult> GetMyRating(string recipeId)
    {
        var userId = HttpContext.GetUserId();
        if (userId is null)
        {
            return NotFound();
        }

        var ratingResult = await _ratingQueries.GetMyRatingAsync(userId, recipeId);
        if (ratingResult.IsFailed)
        {
            return NotFound();
        }

        return Ok(ratingResult.Value);
    }
}