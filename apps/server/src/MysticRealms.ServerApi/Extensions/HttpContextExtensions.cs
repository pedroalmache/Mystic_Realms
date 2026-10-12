using System.Security.Claims;

namespace MysticRealms.ServerApi.Extensions;

public static class HttpContextExtensions
{
    public static string? GetUserId(this HttpContext context)
    {
        var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId;
    }

    public static string? GetGamerTag(this HttpContext context)
    {
        return context.User?.FindFirstValue("gamerTag");
    }
}