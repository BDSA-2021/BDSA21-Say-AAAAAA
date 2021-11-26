using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    IPermissionService _permissionService { get; init; }

    public PermissionAuthorizationHandler(IPermissionService permissionService) =>
        _permissionService = permissionService;

    async protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var user = context.User;
        var isPermitted = await _permissionService.IsAllowed(user, requirement.Permission);

        if (isPermitted)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}