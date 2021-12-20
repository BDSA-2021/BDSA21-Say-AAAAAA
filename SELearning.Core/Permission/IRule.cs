using System.Security.Claims;
using SELearning.Core.Collections;

namespace SELearning.Core.Permission;

public interface IRule
{
    Task<bool> IsAllowed(IDynamicDictionaryRead context, Permission permission);
}
