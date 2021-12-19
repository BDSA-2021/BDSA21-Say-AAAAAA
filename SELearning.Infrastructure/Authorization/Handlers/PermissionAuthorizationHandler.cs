using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SELearning.Core.Credibility;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;


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
    private readonly IEnumerable<IPolicyPipelineOperation> _dataPipeline;
    private readonly ILogger<CredibilityAuthorizationHandler>? _logger;
    public PermissionAuthorizationHandler(IPermissionService permissionService, IEnumerable<IPolicyPipelineOperation> dataPipeline, ILogger<CredibilityAuthorizationHandler>? logger = null)
    {
        _permissionService = permissionService;
        _dataPipeline = dataPipeline;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // Prepare authorization context
        var permissionContext = new PermissionAuthorizationContext(context.User, requirement.Permissions);
        foreach (IPolicyPipelineOperation operation in _dataPipeline)
            await operation.Invoke(permissionContext);

        // Evaluate permission
        bool isPermitted = await _permissionService.IsAllowed(permissionContext.Data, requirement.Permissions);

        _logger?.LogDebug($"User {context.User.GetUserId()} is permitted access: {isPermitted}");

        if (isPermitted)
            context.Succeed(requirement);
        else
            context.Fail();
    }
}