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
        _policyProvider = new PermissionPolicyProvider(Options.Create<AuthorizationOptions>(new AuthorizationOptions()), new PermissionCredibilityService());
    }

    [Theory]
    [InlineData("PermissionCreateComment", 1)]
    [InlineData("PermissionCreateComment OR PermissionCreateComment", 2)]
    public async Task GetPolicyAsync_ProvideKnownPolicy_ReturnPolicyWithPermissionRequirement(string permissions, int expectedNumPermissions)
    {
        var result = await _policyProvider.GetPolicyAsync(permissions);

        Assert.IsType<CredibilityPermissionRequirement>(result?.Requirements[0]);

        var requirement = (CredibilityPermissionRequirement)result?.Requirements[0]!;
        Assert.Equal(expectedNumPermissions, requirement.RequiredCredibilityScores.Count);
    }

    [Fact]
    public async Task GetPolicyAsync_ProvideUnknownPolicy_ReturnNull()
    {
        var result = await _policyProvider.GetPolicyAsync("PermissionUnknownPolicy");

        Assert.Null(result);
    }
}