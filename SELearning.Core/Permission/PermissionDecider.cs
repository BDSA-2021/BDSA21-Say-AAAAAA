namespace SELearning.Core.Permission;

public class PermissionDecider : IPermissionService
{
    public Task<bool> IsAllowed(object user, Permission requestedPermission)
    {
        throw new NotImplementedException();
    }
}