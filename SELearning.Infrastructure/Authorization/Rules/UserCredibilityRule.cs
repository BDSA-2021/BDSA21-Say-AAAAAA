using System.Security.Claims;
using SELearning.Core.Collections;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;


public class UserCredibilityRule : IRule, IResourceRule
{
    public Type EvaluateableType => typeof(object);

    [Obsolete]
    public Task<bool> IsAllowed(ClaimsPrincipal user, Permission permission)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, Permission permission)
    {
        IReadOnlyDictionary<Permission, int> permissionScores = context.Get<IReadOnlyDictionary<Permission, int>>("RequiredCredibilityScores");
        int requiredCredScore = permissionScores[permission];

        int userCredScore = context.Get<int>("UserCredibilityScore");
        
        return await Task.Run<bool>(() => userCredScore >= requiredCredScore);
    }

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, Permission permission, object resource)
    {
        if(!IsEvaluateable(resource))
            return await Task.Run<bool>(() => false);

        return await IsAllowed(context, permission);
    }

    public bool IsEvaluateable(object resource)
    {
        return EvaluateableType.IsInstanceOfType(resource);
    }
}