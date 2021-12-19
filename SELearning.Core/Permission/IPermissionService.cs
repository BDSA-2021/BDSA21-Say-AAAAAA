using System.Security.Claims;
using SELearning.Core.Collections;

namespace SELearning.Core.Permission;

/// <summary>
/// Defines a service to decide if a user is allowed to a perform an action where an associated permission is requried
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Checks if the user is allowed to perform an action with the specified permission.
    /// </summary>
    /// <param name="user">Instance of the user</param>
    /// <param name="requestedPermission">The requested permission</param>
    /// <returns>Returns true if the user is allowed and false if not</returns>
    Task<bool> IsAllowed(ClaimsPrincipal user, Permission requestedPermission);

    Task<bool> IsAllowed(IDynamicDictionaryRead context, IEnumerable<Permission> requestedPermissions);
}