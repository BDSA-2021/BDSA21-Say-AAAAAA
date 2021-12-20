using System.Security.Claims;
using SELearning.Core.Collections;
using SELearning.Core.Credibility;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization.Pipeline.Operations;

public class CredibilityOperation : IAuthorizationContextPipelineOperation
{
    private readonly IProvider<IPermissionCredibilityService> _permCredibilityServiceProvider;
    private readonly IProvider<ICredibilityService> _credServiceProvider;

    public CredibilityOperation(IProvider<IPermissionCredibilityService> permCredibilityServiceProvider,
        IProvider<ICredibilityService> credServiceProvider)
    {
        _permCredibilityServiceProvider = permCredibilityServiceProvider;
        _credServiceProvider = credServiceProvider;
    }

    public async Task Invoke(PermissionAuthorizationContext context)
    {
        await GetPermissionCredibilityScores(context.Data, context.RequestedPermissions);
        await GetCredibilityFromUser(context.Data, context.User);
    }

    private async Task GetCredibilityFromUser(IDynamicDictionary data, ClaimsPrincipal user)
    {
        var credService = _credServiceProvider.Get();

        var userCredScore = await credService.GetCredibilityScore(user);
        data.Set("UserCredibilityScore", userCredScore);
    }

    private async Task GetPermissionCredibilityScores(IDynamicDictionary data,
        IEnumerable<Permission> requestedPermission)
    {
        var credService = _permCredibilityServiceProvider.Get();
        Dictionary<Permission, int> credScores = new();

        foreach (var permission in requestedPermission)
        {
            credScores.Add(permission, await credService.GetRequiredCredibility(permission));
        }

        data.Set<IReadOnlyDictionary<Permission, int>>("RequiredCredibilityScores", credScores);
    }
}
