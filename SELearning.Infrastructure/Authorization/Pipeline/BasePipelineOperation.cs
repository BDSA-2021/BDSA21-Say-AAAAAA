using System.Security.Claims;
using SELearning.Core.Collections;

namespace SELearning.Infrastructure.Authorization;

public abstract class BasePipelineOperation : IPolicyPipelineOperation
{
    public abstract Task Invoke(PermissionAuthorizationContext context);

}