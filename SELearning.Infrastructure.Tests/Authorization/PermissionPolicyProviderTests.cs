using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SELearning.Infrastructure.Authorization;
using static SELearning.Core.Permission.Permission;

namespace SELearning.Infrastructure.Tests.Authorization;

public class PermissionPolicyProviderTests
{
    private readonly IAuthorizationPolicyProvider _policyProvider;

    public PermissionPolicyProviderTests()
    {
        _policyProvider =
            new PermissionPolicyProvider(Options.Create(new AuthorizationOptions()));
    }

    [Theory]
    [InlineData("PermissionRequirement PermissionCreateComment", 1)]
    [InlineData("PermissionRequirement PermissionCreateComment OR PermissionCreateContent", 2)]
    public async Task GetPolicyAsync_ProvideKnownPolicy_ReturnPolicyWithPermissionRequirement(string permissions,
        int expectedNumPermissions)
    {
        var result = await _policyProvider.GetPolicyAsync(permissions);

        Assert.IsType<PermissionRequirement>(result?.Requirements[0]);

        var requirement = (PermissionRequirement)result?.Requirements[0]!;
        Assert.Equal(expectedNumPermissions, requirement.Permissions.Count());
    }

    [Fact]
    public async Task GetPolicyAsync_ProvideUnknownPolicy_ReturnNull()
    {
        var result = await _policyProvider.GetPolicyAsync("PermissionUnknownPolicy");

        Assert.Null(result);
    }

    [Theory]
    [InlineData("PermissionCreateComment", CreateComment)]
    [InlineData("PermissionEditAnyComment", EditAnyComment)]
    [InlineData("PermissionCreateContent", CreateContent)]
    [InlineData("PermissionCreateComment OR PermissionEditAnyComment", CreateComment, EditAnyComment)]
    [InlineData("PermissionEditAnyComment OR PermissionRate OR PermissionDeleteAnyContent", EditAnyComment, Rate,
        DeleteAnyContent)]
    [InlineData("")]
    public void PermissionsToPolicyName_GivenPermissions_CreatesProperPolicyName(string expectedPolicyName,
        params Permission[] p)
    {
        var policyName = PermissionPolicyProvider.PermissionsToPolicyName(p);
        Assert.Equal(expectedPolicyName, policyName);
    }

    [Theory]
    [InlineData("PermissionCreateComment", CreateComment)]
    [InlineData("PermissionEditAnyComment", EditAnyComment)]
    [InlineData("PermissionCreateContent", CreateContent)]
    [InlineData("PermissionCreateComment OR PermissionEditAnyComment", CreateComment, EditAnyComment)]
    [InlineData("PermissionEditAnyComment OR PermissionRate OR PermissionDeleteAnyContent", EditAnyComment, Rate,
        DeleteAnyContent)]
    [InlineData("")]
    public void TryParsePolicyPermissions_GivenPolicyName_ParsesProperPermissions(string policyName,
        params Permission[] expectedPermissions)
    {
        var succeeded = PermissionPolicyProvider.TryParsePolicyPermissions(policyName, out var permissions);
        Assert.True(succeeded, $"Failed on '{policyName}'");
        Assert.Equal(expectedPermissions, permissions);
    }

    [Theory]
    [InlineData("PermissionThatDoesNotExist")]
    [InlineData("EditOwnComment")]
    public void TryParsePolicyPermissions_GivenInvalidPolicyName_Fails(string policyName)
    {
        var succeeded = PermissionPolicyProvider.TryParsePolicyPermissions(policyName, out _);
        Assert.False(succeeded);
    }

    [Fact]
    public void TryParsePolicyPermissionsRequirement_GivenPermissionRequirementPolicy_ReturnsPermissionRequirement()
    {
        const string policyName = "PermissionRequirement PermissionCreateComment OR PermissionEditAnyComment";
        var expectedRequirement = new PermissionRequirement(CreateComment, EditAnyComment);
        var success =
            PermissionPolicyProvider.TryParsePolicyPermissionsRequirement(policyName, out var parsedRequirement);

        Assert.True(success);
        Assert.IsType<PermissionRequirement>(parsedRequirement);
        Assert.Equal(expectedRequirement.Permissions, parsedRequirement.Permissions);
    }

    [Fact]
    public void
        TryParsePolicyPermissionsRequirement_GivenResourcePermissionRequirementPolicy_ReturnsResourcePermissionRequirement()
    {
        const string policyName = "ResourcePermissionRequirement PermissionCreateComment OR PermissionEditAnyComment";
        var expectedRequirement = new ResourcePermissionRequirement(CreateComment, EditAnyComment);
        var success =
            PermissionPolicyProvider.TryParsePolicyPermissionsRequirement(policyName, out var parsedRequirement);

        Assert.True(success);
        Assert.IsType<ResourcePermissionRequirement>(parsedRequirement);
        Assert.Equal(expectedRequirement.Permissions, parsedRequirement.Permissions);
    }

    [Fact]
    public void TryParsePolicyPermissionsRequirement_GivenNoRequirementPolicy_Fails()
    {
        const string policyName = "PermissionCreateComment OR PermissionEditAnyComment";
        var success =
            PermissionPolicyProvider.TryParsePolicyPermissionsRequirement(policyName, out _);

        Assert.False(success);
    }

    [Fact]
    public void PermissionsToRequirementPolicyName_GivenRequirement_PrependsRequirementName()
    {
        var policyName =
            PermissionPolicyProvider.PermissionsToRequirementPolicyName<ResourcePermissionRequirement>(CreateComment);
        Assert.Equal("ResourcePermissionRequirement PermissionCreateComment", policyName);
    }
}
