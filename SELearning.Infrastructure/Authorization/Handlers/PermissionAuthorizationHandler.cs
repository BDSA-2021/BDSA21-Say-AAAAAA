using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SELearning.Core.Credibility;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;


/// <summary>
/// Evaluates the credibility permission requirement and notifies the Authorization context about the result.
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionService _permissionService;
    private readonly IPolicyPipelineOperation _dataPipeline;
    private readonly ILogger<CredibilityAuthorizationHandler>? _logger;
    public PermissionAuthorizationHandler(IPermissionService permissionService, IPolicyPipelineOperation dataPipeline, ILogger<CredibilityAuthorizationHandler>? logger = null)
    {
        _permissionService = permissionService;
        _dataPipeline = dataPipeline;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
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

        _logger?.LogDebug($"User {context.User.GetUserId()} is permitted access: {isPermitted}");

        if (isPermitted)
            context.Succeed(requirement);
        else
            context.Fail();
    }

    private bool IsModerator(PermissionAuthorizationContext context)
        => context.Data.Get<bool>("IsModerator");
}