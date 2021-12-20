using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SELearning.Core.Permission;
using SELearning.Infrastructure.Authorization.Pipeline;

namespace SELearning.Infrastructure.Authorization.Handlers;

/// <summary>
/// Evaluates the credibility permission requirement and notifies the Authorization context about the result.
/// This is an ASP.NET authorization handler that connects ASP.NET to our own
/// permission-based access rules. This handler specifically handles permissions
/// that are NOT resource-based (i.e. "simple" permissions like "user can create
/// content").
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionService _permissionService;
    private readonly IEnumerable<IAuthorizationContextPipelineOperation> _dataPipeline;
    private readonly ILogger<PermissionAuthorizationHandler>? _logger;

    public PermissionAuthorizationHandler(IPermissionService permissionService,
        IEnumerable<IAuthorizationContextPipelineOperation> dataPipeline,
        ILogger<PermissionAuthorizationHandler>? logger = null)
    {
        _permissionService = permissionService;
        _dataPipeline = dataPipeline;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Prepare authorization context
        var permissionContext = new PermissionAuthorizationContext(context.User, requirement.Permissions);
        foreach (var operation in _dataPipeline)
            await operation.Invoke(permissionContext);

        // Evaluate permission
        var isPermitted = await _permissionService.IsAllowed(permissionContext.Data, requirement.Permissions);

        _logger?.LogDebug("User {userId} is permitted access: {isPermitted}", context.User.GetUserId(), isPermitted);

        if (isPermitted)
            context.Succeed(requirement);
        else
            context.Fail();
    }
}
