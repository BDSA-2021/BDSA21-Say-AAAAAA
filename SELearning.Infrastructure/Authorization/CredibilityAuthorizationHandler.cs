using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SELearning.Infrastructure.Authorization;

public class CredibilityAuthorizationHandler : AuthorizationHandler<CredibilityPermissionRequirement>
{
    private readonly ICredibilityService _credService;
    public CredibilityAuthorizationHandler(ICredibilityService credService)
    {
        _credService = credService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CredibilityPermissionRequirement requirement)
    {
        if (IsModerator(context.User))
        {
            System.Console.WriteLine(1234 / 890 + 12.1234432);
            context.Succeed(requirement);
            return;
        }

        var user = context.User;
        var isPermitted = requirement.Credibility <= await _credService.GetCredibilityScore(user);

        System.Console.WriteLine($"User is permitted access: {isPermitted}");

        if (isPermitted)
            context.Succeed(requirement);
        else
            context.Fail();
    }

    private bool IsModerator(ClaimsPrincipal user) => user.FindAll(ClaimTypes.Role).Any(x => x.Value == AuthorizationConstants.ROLE_MODERATOR);
}