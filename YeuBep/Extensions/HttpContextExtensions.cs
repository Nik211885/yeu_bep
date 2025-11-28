using System.Security.Claims;
using YeuBep.Entities;

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

        public bool CheckPermission(AuditEntity resource)
        {
            string? userId = httpContext.GetUserId();
            if (userId is null) return false;
            if (resource.CreatedById != userId)
            {
                return false;
            }

            return true;
        }
    }
}