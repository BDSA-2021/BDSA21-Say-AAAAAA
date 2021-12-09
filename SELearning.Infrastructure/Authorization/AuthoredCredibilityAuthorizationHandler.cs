using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class AuthoredCredibilityAuthorizationHandler : AuthorizationHandler<CredibilityPermissionRequirement, IAuthored>
{
    private readonly IProvider<ICredibilityService> _credService;
    private readonly ILogger<AuthoredCredibilityAuthorizationHandler>? _logger;
    public AuthoredCredibilityAuthorizationHandler(IProvider<ICredibilityService> credService, ILogger<AuthoredCredibilityAuthorizationHandler>? logger = null)
    {
        _credService = credService;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CredibilityPermissionRequirement requirement, IAuthored resource)
    {
        var credService = _credService.Get();
        if (IsModerator(context.User))
        {
            _logger?.LogDebug($"User {context.User.GetUserId()} is a moderator");
            context.Succeed(requirement);
            return;
        }

        var user = context.User;
        var userCredibilityScore = await credService.GetCredibilityScore(user);
        var isPermitted = requirement.RequiredCredibilityScores.Any(req => RequirementIsSatisfied(req, user, resource, userCredibilityScore));

        if (isPermitted)
            context.Succeed(requirement);
        else
            context.Fail();
    }

    private bool IsModerator(ClaimsPrincipal user)
        => user.FindAll(ClaimTypes.Role).Any(x => x.Value == AuthorizationConstants.ROLE_MODERATOR);

    private bool RequirementIsSatisfied((Permission, int) permWithScore, ClaimsPrincipal user, IAuthored resource, int userCredibilityScore)
    {
        var (permission, requiredCredScore) = permWithScore;

        var isPermitted = requiredCredScore <= userCredibilityScore;
        if (permission.ActsOnAuthorOnly())
            isPermitted &= resource.Author.Id == user.GetUserId();

        _logger?.LogDebug($"User {user.GetUserId()} has access: {isPermitted}");

        return isPermitted;
    }
}