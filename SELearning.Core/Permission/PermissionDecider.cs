using System.Security.Claims;

namespace SELearning.Core.Permission;

public class PermissionDecider : IPermissionService
{
    private readonly IDictionary<Permission, IEnumerable<IRule>> _permissions;

    public PermissionDecider(IDictionary<Permission, IEnumerable<IRule>> permissionRules)
    {
        _permissions = permissionRules;
    }

    public async Task<bool> IsAllowed(ClaimsPrincipal user, Permission requestedPermission)
    {
        if (!PermissionHasRules(requestedPermission))
            return true;

        return (await Task.WhenAll(_permissions[requestedPermission].Select(rule => rule.IsAllowed(user, requestedPermission))))
                                                                    .All(isAllowed => isAllowed);
    }

    private bool PermissionHasRules(Permission p) => _permissions.ContainsKey(p) && _permissions[p].Count() != 0;
}
