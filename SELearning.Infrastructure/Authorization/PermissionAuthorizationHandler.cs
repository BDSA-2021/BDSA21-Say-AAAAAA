using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Authorization handler for SELearning specific permissions. 
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    IPermissionService _permissionService { get; init; }

    public PermissionAuthorizationHandler(IPermissionService permissionService) =>
        _permissionService = permissionService;

    /// <summary>
    /// Determines if the user from the Authorization context has the required permission
    /// </summary>
    /// <param name="context">Context of the authorization</param>
    /// <param name="requirement">The requirement to validate the user with</param>
    /// <returns></returns>
    async protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // TODO: Add default check if the user is a moderator... it should be a claims thing... i think

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