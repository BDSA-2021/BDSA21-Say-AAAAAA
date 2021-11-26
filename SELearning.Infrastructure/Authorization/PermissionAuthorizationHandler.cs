using Microsoft.AspNetCore.Authorization;

namespace SELearning.Infrastructure.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        throw new NotImplementedException();
    }
}