using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;

namespace SELearning.Infrastructure.Tests.Authorization;

public class PermissionPolicyProviderTests
{
    private IAuthorizationPolicyProvider _policyProvider;

    public PermissionPolicyProviderTests()
    {
        _policyProvider = new PermissionPolicyProvider();
    }

    [Fact]
    public async Task GetPolicyAsync_ProvideEmptyString_ReturnDefaultPolicy()
    {
        var result = await _policyProvider.GetPolicyAsync("");

        Assert.Equal(new PermissionRequirement(Permission.CreateComment), result?.Requirements[0]);
    }

    [Fact]
    public async Task GetPolicyAsync_ProvideKnownPolicy_ReturnPolicyWithPermissionRequirement()
    {
        var result = await _policyProvider.GetPolicyAsync("PermissionCreateDocument");

        Assert.Equal(new PermissionRequirement(Permission.CreateComment), result?.Requirements[0]);
    }
}