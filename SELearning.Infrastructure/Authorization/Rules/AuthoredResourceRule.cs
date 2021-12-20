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

        IAuthored authoredRessource = (IAuthored)resource;
        string userId = context.Get<string>("UserId");

        return userId == authoredRessource.Author.Id;
    }
}