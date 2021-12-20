using SELearning.Core.Collections;

namespace SELearning.Core.Permission;

public class PermissionDecider : IPermissionService, IResourcePermissionService
{
    private readonly IDictionary<Permission, IEnumerable<IRule>> _permissions;
    private readonly IDictionary<Permission, IEnumerable<IResourceRule>> _resourcePermissions;

    public PermissionDecider(
        IDictionary<Permission, IEnumerable<IRule>> permissionRules,
        IDictionary<Permission, IEnumerable<IResourceRule>> resourcePermissions)
    {
        _permissions = permissionRules;
        _resourcePermissions = resourcePermissions;
    }

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, IEnumerable<Permission> requestedPermissions)
    {
        if (UserIsAModerator(context))
            return true;

        foreach (var requestedPermission in requestedPermissions)
        {
            if (!PermissionHasRules(requestedPermission, _permissions))
                return true;

            var result = (await Task.WhenAll(_permissions[requestedPermission]
                    .Select(rule => rule.IsAllowed(context, requestedPermission))))
                .All(isAllowed => isAllowed);

            if (result)
                return true;
        }

        return false;
    }

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, IEnumerable<Permission> requestedPermissions,
        object ressource)
    {
        if (UserIsAModerator(context))
            return true;

        foreach (var requestedPermission in requestedPermissions)
        {
            if (!PermissionHasRules(requestedPermission, _resourcePermissions))
                return true;

            var result = (await Task.WhenAll(_resourcePermissions[requestedPermission]
                    .Where(x => x.IsEvaluateable(ressource))
                    .Select(rule => rule.IsAllowed(context, requestedPermission, ressource))))
                .All(isAllowed => isAllowed);

            if (result)
                return true;
        }

        return false;
    }

    private static bool UserIsAModerator(IDynamicDictionaryRead context) => context.Get<bool>("IsModerator");

    private static bool PermissionHasRules<T>(Permission p, IDictionary<Permission, IEnumerable<T>> ruleTables) =>
        ruleTables.ContainsKey(p) && ruleTables[p].Any();
}
