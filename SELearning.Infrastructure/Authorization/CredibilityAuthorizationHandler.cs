using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace SELearning.Infrastructure.Authorization;


/// <summary>
/// Evaluates the credibility permission requirement and notifies the Authorization context about the result.
/// </summary>
public class CredibilityAuthorizationHandler : AuthorizationHandler<CredibilityPermissionRequirement>
{
    private readonly ICredibilityService _credService;
    private readonly ILogger<CredibilityAuthorizationHandler>? _logger;
    public CredibilityAuthorizationHandler(ICredibilityService credService, ILogger<CredibilityAuthorizationHandler>? logger = null)
    {
        _credService = credService;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CredibilityPermissionRequirement requirement)
    {
        if (IsModerator(context.User))
        {
            _logger?.LogDebug($"User {context.User.GetUserId()} is a moderator");
            context.Succeed(requirement);
            return;
        }

        var user = context.User;
        var userCredibilityScore = await _credService.GetCredibilityScore(user);
        var isPermitted = requirement.RequiredCredibilityScores.Any(requiredScore => requiredScore <= userCredibilityScore);

        _logger?.LogDebug($"User {context.User.GetUserId()} is permitted access: {isPermitted}");

        if (isPermitted)
            context.Succeed(requirement);
        else
            context.Fail();
    }

    private bool IsModerator(ClaimsPrincipal user)
        => user.FindAll(ClaimTypes.Role).Any(x => x.Value == AuthorizationConstants.ROLE_MODERATOR);
}