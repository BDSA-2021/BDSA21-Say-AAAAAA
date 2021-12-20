using SELearning.Core.Collections;

namespace SELearning.Core.Permission;

public interface IResourcePermissionService
{
    Task<bool> IsAllowed(IDynamicDictionaryRead context, IEnumerable<Permission> requestedPermissions, object ressource);
}