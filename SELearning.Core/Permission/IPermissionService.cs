using SELearning.Core.Collections;

namespace SELearning.Core.Permission;

/// <summary>
/// Defines a service to decide if a user is allowed to a perform an action where an associated permission is required
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Checks if the user is allowed to perform an action with the specified permissions.
    /// The permission provided is checked as an or relation.
    /// </summary>
    /// <param name="context">The context that the permissions is evaluated in</param>
    /// <param name="requestedPermissions">The requested permissions</param>
    /// <returns>Returns true if the user is allowed and false if not</returns>
    Task<bool> IsAllowed(IDynamicDictionaryRead context, IEnumerable<Permission> requestedPermissions);
}
