using SELearning.Core.Collections;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization.Rules;

public class UserCredibilityRule : BaseResourceRule, IRule
{
    public UserCredibilityRule()
        : base(typeof(object))
    {
    }

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, Permission permission)
    {
        var permissionScores =
            context.Get<IReadOnlyDictionary<Permission, int>>("RequiredCredibilityScores");
        var requiredCredScore = permissionScores[permission];

        var userCredScore = context.Get<int>("UserCredibilityScore");

        return await Task.Run(() => userCredScore >= requiredCredScore);
    }

    public override async Task<bool> IsAllowed(IDynamicDictionaryRead context, Permission permission, object resource)
    {
        if (!IsEvaluateable(resource))
            return await Task.Run(() => false);

        return await IsAllowed(context, permission);
    }
}
