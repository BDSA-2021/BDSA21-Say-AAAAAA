using SELearning.Core.Collections;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization.Rules;

public abstract class BaseResourceRule : IResourceRule
{
    public Type EvaluateableType { get; }

    public BaseResourceRule(Type evaluateableType)
    {
        EvaluateableType = evaluateableType;
    }

    public abstract Task<bool> IsAllowed(IDynamicDictionaryRead context, Permission permission, object resource);

    public virtual bool IsEvaluateable(object resource) => EvaluateableType.IsInstanceOfType(resource);
}
