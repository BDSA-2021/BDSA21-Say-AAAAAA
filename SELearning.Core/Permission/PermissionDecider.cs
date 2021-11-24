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

        foreach (Rule rule in _permissions[requestedPermission])
        {
            bool isAllowed = await rule(user);
            if (!isAllowed)
                return false;
        }

        return true;
    }

    private bool PermissionHasRules(Permission p) => _permissions.ContainsKey(p) && _permissions[p].Count() != 0;
}