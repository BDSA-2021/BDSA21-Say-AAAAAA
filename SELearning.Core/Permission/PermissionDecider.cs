using System.Security.Claims;
using SELearning.Core.Collections;

namespace SELearning.Core.Permission;

public class PermissionDecider : IPermissionService, IResourcePermissionService
{
    private readonly IDictionary<Permission, IEnumerable<IRule>> _permissions;
    private readonly IDictionary<Permission, IEnumerable<IResourceRule>> _resourcePermissions;

    public PermissionDecider(IDictionary<Permission, IEnumerable<IRule>> permissionRules, IDictionary<Permission, IEnumerable<IResourceRule>> resourcePermissions = null) // TODO: Remove null
    {
        _permissions = permissionRules;
        _resourcePermissions = resourcePermissions;
    }

    public async Task<bool> IsAllowed(ClaimsPrincipal user, Permission requestedPermission)
    {
        if (!PermissionHasRules(requestedPermission, _permissions))
            return true;

        return (await Task.WhenAll(_permissions[requestedPermission].Select(rule => rule.IsAllowed(user, requestedPermission))))
                                                                    .All(isAllowed => isAllowed);
    }

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, IEnumerable<Permission> requestedPermissions)
    {
        if(UserIsAModerator(context))
            return true;

        foreach(Permission requestedPermission in requestedPermissions)
        {
            if (!PermissionHasRules(requestedPermission, _permissions))
                return true;

            foreach(IRule rule in _permissions[requestedPermission])
                if(!(await rule.IsAllowed(context, requestedPermission)))
                    return false;
        }

        return true;
    }

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, IEnumerable<Permission> requestedPermissions, object ressource)
    {
        if(UserIsAModerator(context))
            return true;

        foreach(Permission requestedPermission in requestedPermissions)
        {
            if (!PermissionHasRules(requestedPermission, _resourcePermissions))
                return true;

            foreach(IResourceRule rule in _resourcePermissions[requestedPermission].Where(x => x.IsEvaluateable(ressource)))
                if(!(await rule.IsAllowed(context, requestedPermission, ressource)))
                    return false;
        }

        return true;
    }

    private bool UserIsAModerator(IDynamicDictionaryRead context) => context.Get<bool>("IsModerator");

    private bool PermissionHasRules<T>(Permission p, IDictionary<Permission, T> ruleTables) => _permissions.ContainsKey(p) && _permissions[p].Count() != 0;
}
