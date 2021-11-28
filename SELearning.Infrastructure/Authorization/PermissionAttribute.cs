using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Authorization attribute with the required permission
/// </summary>
public class PermissionAttribute : AuthorizeAttribute
{
    public PermissionAttribute(Permission p)
    {
        var permissionString = Enum.GetName(typeof(Permission), p);
        Policy = $"{AuthorizationConstants.POLICY_PREFIX}{permissionString}";
    }
}