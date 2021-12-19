using System.Security.Claims;
using SELearning.Core.Collections;

namespace SELearning.Core.Permission;

public interface IResourceRule
{
    public Type EvaluateableType { get; }

    bool IsEvaluateable(object resource);

    Task<bool> IsAllowed(IDynamicDictionaryRead context, Permission permission, in object resource);
}
