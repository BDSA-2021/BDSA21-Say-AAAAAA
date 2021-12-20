using SELearning.Core.Collections;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization.Rules;

public class AuthoredResourceRule : BaseResourceRule
{
    public AuthoredResourceRule()
        : base(typeof(IAuthored))
    {
    }

    public override async Task<bool> IsAllowed(IDynamicDictionaryRead context, Permission permission, object resource)
    {
        if (!IsEvaluateable(resource))
            return await Task.Run(() => false);

        var authoredResource = (IAuthored) resource;
        var userId = context.Get<string>("UserId");
        var userCredibilityScore = context.Get<int>("UserCredibilityScore");
        var permissionCredScoreLevels = context.Get<IReadOnlyDictionary<Permission, int>>("RequiredCredibilityScores");

        var isPermitted = permissionCredScoreLevels[permission] <= userCredibilityScore;
        if (permission.ActsOnAuthorOnly())
            isPermitted &= authoredResource.Author.Id == userId;


        return isPermitted;
    }
}
