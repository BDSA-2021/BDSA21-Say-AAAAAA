using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SELearning.Core.Permission;
using SELearning.Infrastructure.Authorization.Pipeline;

namespace SELearning.Infrastructure.Authorization.Handlers;

/// This is an ASP.NET authorization handler that connects ASP.NET to our own
/// permission-based access rules. This handler specifically handles permissions
/// that are resource-based (i.e. if the permission needs to check that the
/// author of a comment is the user requesting to delete it).
public class ResourcePermissionAuthorizationHandler : AuthorizationHandler<ResourcePermissionRequirement, IAuthored>
{
    private readonly ILogger<ResourcePermissionAuthorizationHandler>? _logger;
    private readonly IResourcePermissionService _permissionService;
    private readonly IEnumerable<IAuthorizationContextPipelineOperation> _dataPipeline;

    public ResourcePermissionAuthorizationHandler(
        IResourcePermissionService permissionService,
        IEnumerable<IAuthorizationContextPipelineOperation> dataPipeline,
        ILogger<ResourcePermissionAuthorizationHandler>? logger = null)
    {
        _permissionService = permissionService;
        _dataPipeline = dataPipeline;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ResourcePermissionRequirement requirement, IAuthored resource)
    {
        // Prepare authorization context
        var permissionContext = new PermissionAuthorizationContext(context.User, requirement.Permissions);
        foreach (var operation in _dataPipeline)
            await operation.Invoke(permissionContext);

        // Evaluate permission
        var isPermitted =
            await _permissionService.IsAllowed(permissionContext.Data, requirement.Permissions, resource);

        _logger?.LogDebug("User {userId} has access: {isPermitted}", context.User.GetUserId(), isPermitted);

        if (isPermitted)
            context.Succeed(requirement);
        else
            context.Fail();
    }

    private bool IsModerator(PermissionAuthorizationContext context)
        => context.Data.Get<bool>("IsModerator");
}
