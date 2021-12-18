using System.Security.Claims;
using SELearning.Core.Collections;
using SELearning.Core.Credibility;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class CredibilityOperation : BasePipelineOperation
{
    private readonly IProvider<IPermissionCredibilityService> _permCredibilityServiceProvider;
    private readonly IProvider<ICredibilityService> _credServiceProvider;

    public CredibilityOperation(IProvider<IPermissionCredibilityService> permCredibilityServiceProvider, IProvider<ICredibilityService> credServiceProvider)
    {
        _permCredibilityServiceProvider = permCredibilityServiceProvider;
        _credServiceProvider = credServiceProvider;    
    }

    public override async Task Invoke(PermissionAuthorizationContext context)
    {
        await GetPermissionCredibilityScores(context.Data, context.RequestedPermissions);
        await GetCredibilityFromUser(context.Data, context.User);
    }

    private async Task GetCredibilityFromUser(IDynamicDictionary data, ClaimsPrincipal user)
    {
        ICredibilityService credService = _credServiceProvider.Get();

        int userCredScore = await credService.GetCredibilityScore(user);
        data.Set("UserCredibilityScore", userCredScore);
    }

    private async Task GetPermissionCredibilityScores(IDynamicDictionary data, IEnumerable<Permission> requestedPermission)
    {
        IPermissionCredibilityService credService = _permCredibilityServiceProvider.Get();
        IList<int> credScores = new List<int>();

        foreach (Permission permission in requestedPermission)
        {
            credScores.Add(await credService.GetRequiredCredibility(permission));
        }

        data.Set("RequiredCredibilityScores", credScores);
    }
}