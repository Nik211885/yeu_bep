using Microsoft.AspNetCore.Mvc;
using YeuBep.Extensions;
using YeuBep.Services;
using YeuBep.ViewModels.Rating;

namespace YeuBep.Controllers.Apis;

[ApiController]
[Route("api/rating")]
public class RatingApiController : ControllerBase
{
    private readonly RatingServices _ratingServices;
    private readonly ILogger<RatingApiController> _logger;

    public RatingApiController(RatingServices ratingServices, ILogger<RatingApiController> logger)
    {
        _ratingServices = ratingServices;
        _logger = logger;
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
}