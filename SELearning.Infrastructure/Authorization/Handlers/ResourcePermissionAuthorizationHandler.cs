using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SELearning.Core.Credibility;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// This is an ASP.NET authorization handler that connects ASP.NET to our own
/// permission-based access rules. This handler specifically handles permissions
/// that are resource-based (i.e. if the permission needs to check that the
/// author of a comment is the user requesting to delete it).
public class ResourcePermissionAuthorizationHandler : AuthorizationHandler<ResourcePermissionRequirement, IAuthored>
{
    private readonly ILogger<ResourcePermissionAuthorizationHandler>? _logger;
    private readonly IResourcePermissionService _permissionService;
    private readonly IPolicyPipelineOperation _dataPipeline;

    public ResourcePermissionAuthorizationHandler(
        IResourcePermissionService permissionService,
        IPolicyPipelineOperation dataPipeline,
        ILogger<ResourcePermissionAuthorizationHandler>? logger = null)
    {
        _permissionService = permissionService;
        _dataPipeline = dataPipeline;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourcePermissionRequirement requirement, IAuthored resource)
    {
        // Prepare authorization context
        var permissionContext = new PermissionAuthorizationContext(context.User, requirement.Permissions);
        await _dataPipeline.Invoke(permissionContext);

        // Evaluate permission
        bool isPermitted = await _permissionService.IsAllowed(permissionContext.Data, requirement.Permissions, resource);

        _logger?.LogDebug($"User {context.User.GetUserId()} has access: {isPermitted}");

        if (isPermitted)
            context.Succeed(requirement);
        else
            context.Fail();
    }

    private bool IsModerator(PermissionAuthorizationContext context)
        => context.Data.Get<bool>("IsModerator");
}