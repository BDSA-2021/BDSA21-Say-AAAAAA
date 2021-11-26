using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class PermissionAttribute : AuthorizeAttribute
{
    // TODO: @anti, fix this with your upcoming constants thing
    const string POLICY_PREFIX = "Permission";

    public PermissionAttribute(Permission p)
    {
        var permissionString = Enum.GetName(typeof(Permission), p);
        Policy = $"{POLICY_PREFIX}{permissionString}";
    }
}