using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SELearning.Core.Collections;
using SELearning.Core.Permission;
using perm = SELearning.Core.Permission;
using static SELearning.Core.Permission.Permission;

namespace SELearning.Core.Tests.Permission;

public class PermissionDeciderTests
{
    private readonly IDynamicDictionary _context;
    private readonly Dictionary<perm.Permission, IEnumerable<IResourceRule>> _resourceRules;
    private readonly Dictionary<perm.Permission, IEnumerable<IRule>> _permissionRules;

    public PermissionDeciderTests()
    {
        _context = new DynamicDictionary();
        _context.Set("IsModerator", false);

        _resourceRules = new Dictionary<perm.Permission, IEnumerable<IResourceRule>>
        {
            {CreateComment, new List<MockRule> {new(false), new(true)}},
            {EditAnyComment, new List<MockRule> {new(true), new(true)}},
            {Rate, new List<MockRule> {new(true), new(false)}},
            {EditSection, new List<MockRule> {new(true), new(false), new(true)}},
            {CreateSection, new List<MockRule> {new(true), new(true), new(true)}},
            {DeleteAnyComment, new List<MockRule> {new(true, false), new(true, false), new(true, false)}}
        };

        _permissionRules = new Dictionary<perm.Permission, IEnumerable<IRule>>
        {
            {CreateComment, new List<MockRule> {new(false), new(true)}},
            {EditAnyComment, new List<MockRule> {new(true), new(true)}},
            {Rate, new List<MockRule> {new(true), new(false)}},
            {EditSection, new List<MockRule> {new(true), new(false), new(true)}},
            {CreateSection, new List<MockRule> {new(true), new(true), new(true)}},
            {DeleteAnyComment, new List<MockRule> {new(true, false), new(true, false), new(true, false)}}
        };
    }

    [Fact]
    public async Task IsAllowed_OneResourcePermissionEvaluatedToTrue_ReturnTrue()
    {
        var permissionDecider = new PermissionDecider(null!, _resourceRules);

        var result = await permissionDecider.IsAllowed(_context,
            new List<perm.Permission> { CreateComment, EditAnyComment, Rate, EditSection, CreateSection }, "Abe");

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_NoResourceRulesToEvaluatePermission_ReturnTrue()
    {
        var permissionDecider = new PermissionDecider(null!, _resourceRules);

        var result = await permissionDecider.IsAllowed(_context, new List<perm.Permission> { DeleteAnyComment }, "Abe");

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_NoResourcePermissionsEvaluatedAsTrue_ReturnFalse()
    {
        var permissionDecider = new PermissionDecider(null!, _resourceRules);

        var result = await permissionDecider.IsAllowed(_context,
            new List<perm.Permission> { CreateComment, Rate, EditSection }, "Abe");

        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_ResourcePermissionUserIsAModerator_ReturnTrue()
    {
        var permissionDecider = new PermissionDecider(null!, _resourceRules);
        _context.Set("IsModerator", true);

        var result = await permissionDecider.IsAllowed(_context,
            new List<perm.Permission> { CreateComment, EditAnyComment, Rate, EditSection, CreateSection }, "Abe");

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_OnePermissionEvaluatedToTrue_ReturnTrue()
    {
        var permissionDecider = new PermissionDecider(_permissionRules, null!);

        var result = await permissionDecider.IsAllowed(_context,
            new List<perm.Permission> { CreateComment, EditAnyComment, Rate, EditSection, CreateSection });

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_NoPermissionsEvaluatedAsTrue_ReturnFalse()
    {
        var permissionDecider = new PermissionDecider(_permissionRules, null!);

        var result =
            await permissionDecider.IsAllowed(_context, new List<perm.Permission> { CreateComment, Rate, EditSection });

        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_PermissionUserIsAModerator_ReturnTrue()
    {
        var permissionDecider = new PermissionDecider(_permissionRules, null!);
        _context.Set("IsModerator", true);

        var result = await permissionDecider.IsAllowed(_context,
            new List<perm.Permission> { CreateComment, EditAnyComment, Rate, EditSection, CreateSection });

        Assert.True(result);
    }
}

public class MockRule : IRule, IResourceRule
{
    private readonly bool _returnResult;
    private readonly bool _returnTypeEvaluationResult;

    public MockRule(bool returnResult, bool returnTypeEvaluationResult = true)
    {
        _returnResult = returnResult;
        _returnTypeEvaluationResult = returnTypeEvaluationResult;
    }

    public Type EvaluateableType => typeof(object);

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, perm.Permission permission)
    {
        return await Task.Run(() => _returnResult);
    }

    public async Task<bool> IsAllowed(IDynamicDictionaryRead context, perm.Permission permission, object resource)
    {
        return await Task.Run(() => _returnResult);
    }

    public bool IsEvaluateable(object resource)
    {
        return _returnTypeEvaluationResult;
    }
}
