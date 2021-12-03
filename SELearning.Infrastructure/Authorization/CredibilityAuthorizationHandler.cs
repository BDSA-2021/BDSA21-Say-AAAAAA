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
        // TODO: Add default check if the user is a moderator... it should be a claims thing... i think

        var user = context.User;
        var isPermitted = requirement.Credibility <= await _credService.GetCredibilityScore(user);

        if (isPermitted)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}