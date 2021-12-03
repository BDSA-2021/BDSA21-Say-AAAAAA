using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SELearning.Infrastructure.Authorization;

namespace SELearning.Infrastructure.Tests.Authorization;

public class PermissionPolicyProviderTests
{
    private IAuthorizationPolicyProvider _policyProvider;

    public PermissionPolicyProviderTests()
    {
        _policyProvider = new PermissionPolicyProvider(Options.Create<AuthorizationOptions>(new AuthorizationOptions()), null);
    }

    [Fact]
    public async Task GetPolicyAsync_ProvideEmptyString_ReturnNull()
    {
        var result = await _policyProvider.GetPolicyAsync("");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetPolicyAsync_ProvideKnownPolicy_ReturnPolicyWithPermissionRequirement()
    {
        var result = await _policyProvider.GetPolicyAsync("PermissionCreateComment");

        Assert.Equal(new PermissionRequirement(Permission.CreateComment), result?.Requirements[0]);
    }

    [Fact]
    public async Task GetPolicyAsync_ProvideUnknownPolicy_ReturnNull()
    {
        var result = await _policyProvider.GetPolicyAsync("PermissionUnknownPolicy");

        Assert.Null(result);
    }
}