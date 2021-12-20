using SELearning.Core.Collections;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class AuthoredResourceRule : BaseResourceRule
{
    public AuthoredResourceRule()
        : base(typeof(IAuthored))
    {

    }
    public override async Task<bool> IsAllowed(IDynamicDictionaryRead context, Permission permission, object resource)
    {
        if (!IsEvaluateable(resource))
            return await Task.Run<bool>(() => false);

        IAuthored authoredResource = (IAuthored)resource;
        string userId = context.Get<string>("UserId");
        int userCredibilityScore = context.Get<int>("UserCredibilityScore");
        IReadOnlyDictionary<Permission, int> permissionCredScoreLevels = context.Get<IReadOnlyDictionary<Permission, int>>("RequiredCredibilityScores");

        var isPermitted = permissionCredScoreLevels[permission] <= userCredibilityScore;
        if (permission.ActsOnAuthorOnly())
            isPermitted &= authoredResource.Author.Id == userId;


        return isPermitted;
    }
}