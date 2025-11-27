using Microsoft.AspNetCore.Mvc;
using YeuBep.Extends;

namespace YeuBep.Controllers.Apis;
[ApiController]
[Route("api/extend")]
public class ExtendApiController : ControllerBase
{
    private readonly ILogger<ExtendApiController> _logger;
    private readonly CloudinaryServices  _cloudinaryServices;

    public ExtendApiController(ILogger<ExtendApiController> logger, CloudinaryServices cloudinaryServices)
    {
        _logger = logger;
        _cloudinaryServices = cloudinaryServices;
    }

    [HttpGet("url-upload")]
    public IActionResult GetUrlUploadFile()
    {
        var url = _cloudinaryServices.GetUrlUploadFileBySignature();
        return Ok(new {url = url});
    }
}