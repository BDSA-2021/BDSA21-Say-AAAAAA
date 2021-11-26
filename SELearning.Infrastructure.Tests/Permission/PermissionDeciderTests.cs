using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SELearning.Core.Permission;
using static SELearning.Core.Permission.Permission;

namespace SELearning.Infrastructure.Tests;

public class PermissionDeciderTests
{
    public static TheoryData<IEnumerable<Rule>> PermissionDeciderFalseScenarios => new TheoryData<IEnumerable<Rule>> {
        { new List<Rule>{ o => Task.Run<bool>(() => false)} },
        { new List<Rule>{ o => Task.Run<bool>(() => true), o => Task.Run<bool>(() => false)} },
        { new List<Rule>{ o => Task.Run<bool>(() => true), o => Task.Run<bool>(() => false), o => Task.Run<bool>(() => true)} }
    };

    [Fact]
    public async Task IsAllowed_NoRulesForRequestedPermission_ReturnTrue()
    {
        // Arrange
        IDictionary<Permission, IEnumerable<Rule>> permissions = new Dictionary<Permission, IEnumerable<Rule>>();
        PermissionDecider permissionDecider = new PermissionDecider(permissions);

        // Act
        bool result = await permissionDecider.IsAllowed(new Object(), CreateComment);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(PermissionDeciderFalseScenarios))]
    public async Task IsAllowed_OneOrMoreRulesEvaluatedToFalse_ReturnFalse(IEnumerable<Rule> rules)
    {
        // Arrange
        IDictionary<Permission, IEnumerable<Rule>> permissions = new Dictionary<Permission, IEnumerable<Rule>>();
        permissions.Add(CreateComment, rules);
        PermissionDecider permissionDecider = new PermissionDecider(permissions);

        // Act
        bool result = await permissionDecider.IsAllowed(new Object(), CreateComment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_AllRulesEvaluatedToTrue_ReturnTrue()
    {
        // Arrange
        IDictionary<Permission, IEnumerable<Rule>> permissions = new Dictionary<Permission, IEnumerable<Rule>>();
        List<Func<object, Task<bool>>> rules = new List<Func<object, Task<bool>>>();
        rules.Add(o => Task.Run<bool>(() => true));
        rules.Add(o => Task.Run<bool>(() => true));
        rules.Add(o => Task.Run<bool>(() => true));
        rules.Add(o => Task.Run<bool>(() => true));

        PermissionDecider permissionDecider = new PermissionDecider(permissions);

        // Act
        bool result = await permissionDecider.IsAllowed(new Object(), CreateComment);

        // Assert
        Assert.True(result);
    }
}