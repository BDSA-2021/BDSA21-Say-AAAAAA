using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using perm = SELearning.Core.Permission;
using static SELearning.Core.Permission.Permission;
using System.Security.Claims;
using SELearning.Core.Permission;
using SELearning.Core.Collections;

namespace SELearning.Core.Tests;

public class PermissionDeciderTests
{
    public static TheoryData<IEnumerable<MockRule>> PermissionDeciderFalseScenarios => new TheoryData<IEnumerable<MockRule>> {
        { new List<MockRule>{ new(false) }},
        { new List<MockRule>{ new(true), new(false) }},
        { new List<MockRule>{ new (true), new(false), new(true)} }
    };

    [Fact]
    public async Task IsAllowed_NoRulesForRequestedPermission_ReturnTrue()
    {
        // Arrange
        IDictionary<perm.Permission, IEnumerable<IRule>> permissions = new Dictionary<perm.Permission, IEnumerable<IRule>>();
        perm.PermissionDecider permissionDecider = new perm.PermissionDecider(permissions);

        // Act
        bool result = await permissionDecider.IsAllowed(new ClaimsPrincipal(), CreateComment);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(PermissionDeciderFalseScenarios))]
    public async Task IsAllowed_OneOrMoreRulesEvaluatedToFalse_ReturnFalse(IEnumerable<MockRule> rules)
    {
        // Arrange
        IDictionary<perm.Permission, IEnumerable<IRule>> permissions = new Dictionary<perm.Permission, IEnumerable<IRule>>();
        permissions.Add(CreateComment, rules);
        PermissionDecider permissionDecider = new PermissionDecider(permissions);

        // Act
        bool result = await permissionDecider.IsAllowed(new ClaimsPrincipal(), CreateComment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_AllRulesEvaluatedToTrue_ReturnTrue()
    {
        // Arrange
        IDictionary<perm.Permission, IEnumerable<IRule>> permissions = new Dictionary<perm.Permission, IEnumerable<IRule>>();
        List<IRule> rules = new List<IRule>();
        rules.Add(new MockRule(true));
        rules.Add(new MockRule(true));
        rules.Add(new MockRule(true));
        rules.Add(new MockRule(true));

        permissions.Add(CreateComment, rules);

        PermissionDecider permissionDecider = new PermissionDecider(permissions);

        // Act
        bool result = await permissionDecider.IsAllowed(new ClaimsPrincipal(), CreateComment);

        // Assert
        Assert.True(result);
    }
}

public class MockRule : perm.IRule
{
    private readonly bool _returnResult;

    public MockRule(bool returnResult)
    {
        _returnResult = returnResult;
    }
    public async Task<bool> IsAllowed(ClaimsPrincipal user, perm.Permission permission)
    {
        return await Task.Run<bool>(() => _returnResult);
    }

    public Task<bool> IsAllowed(IDynamicDictionaryRead context, perm.Permission permission)
    {
        throw new NotImplementedException();
    }
}
