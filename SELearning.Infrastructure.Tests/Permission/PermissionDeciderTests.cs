using System.Threading.Tasks;
using SELearning.Core.Permission;
using static SELearning.Core.Permission.Permission;
using Xunit;

namespace SELearning.Infrastructure.Tests;

public class PermissionDeciderTests
{
    private IPermissionService _permissionService;

    public PermissionDeciderTests()
    {
        PermissionDecider permissionDecider = new PermissionDecider();
        _permissionService = permissionDecider;

        // TODO: Add rules to permissions
    }

    [Fact]
    public async Task IsAllowed_NoRulesForRequestedPermission_ReturnTrue()
    {
        bool result = await _permissionService.IsAllowed(null, CreateComment);

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_OneOrMoreRulesEvaluatedToFalse_ReturnFalse()
    {
        bool result = await _permissionService.IsAllowed(null, CreateComment);

        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_AllRulesEvaluatedToTrue_ReturnTrue()
    {
        bool result = await _permissionService.IsAllowed(null, CreateComment);

        Assert.True(result);
    }
}