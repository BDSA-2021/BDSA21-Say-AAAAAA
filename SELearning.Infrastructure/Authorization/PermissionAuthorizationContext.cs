using System.Security.Claims;
using SELearning.Core.Collections;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class PermissionAuthorizationContext
{
    public IDynamicDictionary Data { get; } = new DynamicDictionary();

    public ClaimsPrincipal User { get; }

    public IEnumerable<Permission> RequestedPermissions { get; }

    public PermissionAuthorizationContext(ClaimsPrincipal currentUser, IEnumerable<Permission> requestedPermissions)
    {
        User = currentUser;
        RequestedPermissions = requestedPermissions;
    }
}
