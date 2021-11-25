using System.Linq;

namespace SELearning.Core.Permission;

public class PermissionDecider : IPermissionService
{
    private readonly IDictionary<Permission, IEnumerable<Rule>> _permissions;


    public PermissionDecider(IDictionary<Permission, IEnumerable<Rule>> permissionRules)
    {
        _permissions = permissionRules;
    }

    public async Task<bool> IsAllowed(object user, Permission requestedPermission)
    {
        if (!PermissionHasRules(requestedPermission))
            return true;

        return (await Task.WhenAll(_permissions[requestedPermission].Select(rule => rule(user))))
                                                                    .All(isAllowed => isAllowed);
    }

    private bool PermissionHasRules(Permission p) => _permissions.ContainsKey(p) && _permissions[p].Count() != 0;
}