using System.Threading.Tasks;
using SELearning.Core.Permission;
using static SELearning.Core.Permission.Permission;
using Xunit;
using System;
using System.Collections.Generic;

namespace SELearning.Infrastructure.Tests;

public class PermissionDeciderTests
{
    public static TheoryData<IEnumerable<Func<object, Task<bool>>>> PermissionDeciderFalseScenarios => new TheoryData<IEnumerable<Func<object, Task<bool>>>> {
            { new List<Func<object, Task<bool>>>{ o => Task.Run<bool>(() => false)} },
            { new List<Func<object, Task<bool>>>{ o => Task.Run<bool>(() => true), o => Task.Run<bool>(() => false)} },
            { new List<Func<object, Task<bool>>>{ o => Task.Run<bool>(() => true), o => Task.Run<bool>(() => false), o => Task.Run<bool>(() => true)} }
        };

    [Fact]
    public async Task IsAllowed_NoRulesForRequestedPermission_ReturnTrue()
    {
        IDictionary<Permission, IEnumerable<Func<object, Task<bool>>>> permissions = new Dictionary<Permission, IEnumerable<Func<object, Task<bool>>>>();
        PermissionDecider permissionDecider = new PermissionDecider(permissions);
        bool result = await permissionDecider.IsAllowed(null, CreateComment);

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(PermissionDeciderFalseScenarios))]
    public async Task IsAllowed_OneOrMoreRulesEvaluatedToFalse_ReturnFalse(IEnumerable<Func<object, Task<bool>>> rules)
    {
        IDictionary<Permission, IEnumerable<Func<object, Task<bool>>>> permissions = new Dictionary<Permission, IEnumerable<Func<object, Task<bool>>>>();
        permissions.Add(CreateComment, rules);
        PermissionDecider permissionDecider = new PermissionDecider(permissions);

        bool result = await permissionDecider.IsAllowed(null, CreateComment);

        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_AllRulesEvaluatedToTrue_ReturnTrue()
    {
        IDictionary<Permission, IEnumerable<Func<object, Task<bool>>>> permissions = new Dictionary<Permission, IEnumerable<Func<object, Task<bool>>>>();
        List<Func<object, Task<bool>>> rules = new List<Func<object, Task<bool>>>();
        rules.Add(o => Task.Run<bool>(() => true));
        rules.Add(o => Task.Run<bool>(() => true));
        rules.Add(o => Task.Run<bool>(() => true));
        rules.Add(o => Task.Run<bool>(() => true));

        PermissionDecider permissionDecider = new PermissionDecider(permissions);
        bool result = await permissionDecider.IsAllowed(null, CreateComment);

        Assert.True(result);
    }
}