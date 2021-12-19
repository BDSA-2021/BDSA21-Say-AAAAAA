using System.Security.Claims;
using SELearning.Core.Collections;

namespace SELearning.Core.Permission;

public interface IRule
{
    [Obsolete]
    Task<bool> IsAllowed(ClaimsPrincipal user, Permission permission);

    Task<bool> IsAllowed(IDynamicDictionaryRead context, Permission permission);
}
