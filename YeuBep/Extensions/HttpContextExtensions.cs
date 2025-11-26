using System.Security.Claims;

namespace YeuBep.Extensions;

public static class HttpContextExtensions
{
    extension(HttpContext httpContext)
    {
        public string? GetUserId()
        {
            var userId = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return userId?.Value;
        }
    }
}