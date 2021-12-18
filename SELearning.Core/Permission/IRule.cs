using System.Security.Claims;

namespace SELearning.Core.Permission;

public interface IRule
{
    Task<bool> IsAllowed(ClaimsPrincipal user, Permission permission);
}
