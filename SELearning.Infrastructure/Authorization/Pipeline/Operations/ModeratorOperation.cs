using System.Security.Claims;
using SELearning.Core.Collections;

namespace SELearning.Infrastructure.Authorization;

public class ModeratorOperation : BasePipelineOperation
{
    public override async Task Invoke(PermissionAuthorizationContext context)
    {
        bool isModerator = context.User.FindAll(ClaimTypes.Role).Any(x => x.Value == AuthorizationConstants.ROLE_MODERATOR);

        context.Data.Set<bool>("IsModerator", isModerator);

        await InvokeNext(context);
    }
}