using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SELearning.Core.Credibility;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class ResourcePermissionAuthorizationHandler : AuthorizationHandler<ResourcePermissionRequirement, IAuthored>
{
    private readonly ILogger<ResourcePermissionAuthorizationHandler>? _logger;
    private readonly IPermissionService _permissionService;
    private readonly IPolicyPipelineOperation _dataPipeline;

    public ResourcePermissionAuthorizationHandler(
        IPermissionService permissionService, 
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

        // If the user is a moderator then everything is allowed
        if (IsModerator(permissionContext))
        {
            _logger?.LogDebug($"User {context.User.GetUserId()} is a moderator");
            context.Succeed(requirement);
            return;
        }

        // Evaluate permission
        bool isPermitted = await _permissionService.IsAllowed(context.User, requirement.Permissions.First());

        _logger?.LogDebug($"User {context.User.GetUserId()} has access: {isPermitted}");

        if (isPermitted)
            context.Succeed(requirement);
        else
            context.Fail();
    }

    private bool IsModerator(PermissionAuthorizationContext context)
        => context.Data.Get<bool>("IsModerator");
}