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

    private readonly IDynamicDictionary _context;
    private readonly Dictionary<perm.Permission, IEnumerable<IResourceRule>> _resourceRules;
    private readonly Dictionary<perm.Permission, IEnumerable<IRule>> _permissionRules;

    public PermissionDeciderTests()
    {
        _context = new DynamicDictionary();
        _context.Set<bool>("IsModerator", false);

        _resourceRules = new Dictionary<perm.Permission, IEnumerable<IResourceRule>>
        {
            {CreateComment, new List<MockRule>{new(false), new(true)}},
            {EditAnyComment, new List<MockRule>{new(true), new(true)}},
            {Rate, new List<MockRule>{new(true), new(false)}},
            {EditSection, new List<MockRule>{new(true), new(false) ,new(true)}},
            {CreateSection, new List<MockRule>{new(true), new(true) ,new(true)}},
            {DeleteAnyComment, new List<MockRule>{new(true, false), new(true, false) ,new(true, false)}},
        };

        _permissionRules = new Dictionary<perm.Permission, IEnumerable<IRule>>
        {
            {CreateComment, new List<MockRule>{new(false), new(true)}},
            {EditAnyComment, new List<MockRule>{new(true), new(true)}},
            {Rate, new List<MockRule>{new(true), new(false)}},
            {EditSection, new List<MockRule>{new(true), new(false) ,new(true)}},
            {CreateSection, new List<MockRule>{new(true), new(true) ,new(true)}},
            {DeleteAnyComment, new List<MockRule>{new(true, false), new(true, false) ,new(true, false)}},
        };
    }

    [Fact]
    public async Task IsAllowed_NoRulesForRequestedPermission_ReturnTrue()
    {
        // Arrange
        IDictionary<perm.Permission, IEnumerable<IRule>> permissions = new Dictionary<perm.Permission, IEnumerable<IRule>>();
        perm.PermissionDecider permissionDecider = new perm.PermissionDecider(permissions, null!);

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
        PermissionDecider permissionDecider = new PermissionDecider(permissions, null!);

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

        PermissionDecider permissionDecider = new PermissionDecider(permissions, null!);

        // Act
        bool result = await permissionDecider.IsAllowed(new ClaimsPrincipal(), CreateComment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_OneResourcePermissionEvaluatedToTrue_ReturnTrue()
    {
        perm.PermissionDecider permissionDecider = new perm.PermissionDecider(null!, _resourceRules);

        bool result = await permissionDecider.IsAllowed(_context, new List<perm.Permission>{CreateComment, EditAnyComment, Rate, EditSection, CreateSection}, "Abe");

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_NoResourceRulesToEvaluatePermission_ReturnTrue()
    {
        perm.PermissionDecider permissionDecider = new perm.PermissionDecider(null!, _resourceRules);

        bool result = await permissionDecider.IsAllowed(_context, new List<perm.Permission>{DeleteAnyComment}, "Abe");

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_NoResourcePermissionsEvaluatedAsTrue_ReturnFalse()
    {
        perm.PermissionDecider permissionDecider = new perm.PermissionDecider(null!, _resourceRules);

        bool result = await permissionDecider.IsAllowed(_context, new List<perm.Permission>{CreateComment, Rate, EditSection}, "Abe");

        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_ResourcePermissionUserIsAModerator_ReturnTrue()
    {
        perm.PermissionDecider permissionDecider = new perm.PermissionDecider(null!, _resourceRules);
        _context.Set<bool>("IsModerator", true);

        bool result = await permissionDecider.IsAllowed(_context, new List<perm.Permission>{CreateComment, EditAnyComment, Rate, EditSection, CreateSection}, "Abe");

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_OnePermissionEvaluatedToTrue_ReturnTrue()
    {
        perm.PermissionDecider permissionDecider = new perm.PermissionDecider(_permissionRules, null!);

        bool result = await permissionDecider.IsAllowed(_context, new List<perm.Permission>{CreateComment, EditAnyComment, Rate, EditSection, CreateSection});

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_NoPermissionsEvaluatedAsTrue_ReturnFalse()
    {
        perm.PermissionDecider permissionDecider = new perm.PermissionDecider(_permissionRules, null!);

        bool result = await permissionDecider.IsAllowed(_context, new List<perm.Permission>{CreateComment, Rate, EditSection});

        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_PermissionUserIsAModerator_ReturnTrue()
    {
        perm.PermissionDecider permissionDecider = new perm.PermissionDecider(_permissionRules, null!);
        _context.Set<bool>("IsModerator", true);

        bool result = await permissionDecider.IsAllowed(_context, new List<perm.Permission>{CreateComment, EditAnyComment, Rate, EditSection, CreateSection});

        Assert.True(result);
    }
}

public class MockRule : perm.IRule, perm.IResourceRule
{
    private readonly bool _returnResult;
    private readonly bool _returnTypeEvaluationResult;

    public MockRule(bool returnResult, bool returnTypeEvaluationResult = true)
    {
        _returnResult = returnResult;
        _returnTypeEvaluationResult = returnTypeEvaluationResult;
    }

    public Type EvaluateableType => typeof(object);

    public async Task<bool> IsAllowed(ClaimsPrincipal user, perm.Permission permission)
    {
        return await Task.Run<bool>(() => _returnResult);
    }

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, perm.Permission permission)
    {
        return await Task.Run<bool>(() => _returnResult);
    }

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, perm.Permission permission, object resource)
    {
        return await Task.Run<bool>(() => _returnResult);
    }

    public bool IsEvaluateable(object resource)
    {
        return  _returnTypeEvaluationResult;
    }
}
