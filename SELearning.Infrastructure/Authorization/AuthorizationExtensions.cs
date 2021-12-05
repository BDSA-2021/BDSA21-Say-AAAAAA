using System.Security.Claims;

namespace SELearning.Infrastructure.Authorization;

public static class AuthorizationExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
