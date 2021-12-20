using System.Security.Claims;

namespace SELearning.Infrastructure.Authorization.Pipeline.Operations;

public class ModeratorOperation : IAuthorizationContextPipelineOperation
{
    public Task Invoke(PermissionAuthorizationContext context)
    {
        var isModerator = context.User.FindAll(ClaimTypes.Role)
            .Any(x => x.Value == AuthorizationConstants.ROLE_MODERATOR);

        context.Data.Set("IsModerator", isModerator);
        context.Data.Set("UserId", context.User.GetUserId()!);

        return Task.CompletedTask;
    }
}
