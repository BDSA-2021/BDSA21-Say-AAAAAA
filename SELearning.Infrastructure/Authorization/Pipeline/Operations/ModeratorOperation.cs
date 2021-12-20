using System.Security.Claims;
using SELearning.Core.Collections;

namespace SELearning.Infrastructure.Authorization;

public class ModeratorOperation : IAuthorizationContextPipelineOperation
{
    public Task Invoke(PermissionAuthorizationContext context)
    {
        bool isModerator = context.User.FindAll(ClaimTypes.Role).Any(x => x.Value == AuthorizationConstants.ROLE_MODERATOR);

        context.Data.Set<bool>("IsModerator", isModerator);
        context.Data.Set<string>("UserId", context.User.GetUserId()!);

        return Task.CompletedTask;
    }
}