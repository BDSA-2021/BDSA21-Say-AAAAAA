using System.Threading.Tasks;
using SELearning.Core.Collections;
using SELearning.Infrastructure.Authorization;
using SELearning.Infrastructure.Authorization.Rules;

namespace SELearning.Infrastructure.Tests.Authorization;

public class UserCredibilityRuleTests
{
    private readonly UserCredibilityRule _rule;
    private readonly DynamicDictionary _context;

    public UserCredibilityRuleTests()
    {
        _rule = new UserCredibilityRule();
        _context = new DynamicDictionary();

        _context.Set<int>("UserCredibilityScore", 500);
        _context.Set<IReadOnlyDictionary<Permission, int>>("RequiredCredibilityScores", new Dictionary<Permission, int>() { { Permission.CreateComment, 1000 }, { Permission.CreateContent, 500 }, { Permission.EditOwnComment, 499 } });
    }

    [Fact]
    public async Task IsAllowed_PermissionCredHigherThanUserCredScore_ReturnFalse()
    {
        bool result = await _rule.IsAllowed(_context, Permission.CreateComment);

        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_PermissionCredIsLowerThanUserCredScore_ReturnTrue()
    {
        bool result = await _rule.IsAllowed(_context, Permission.EditOwnComment);

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_PermissionCredIsEqualToUserCredScore_ReturnTrue()
    {
        bool result = await _rule.IsAllowed(_context, Permission.CreateContent);

        Assert.True(result);
    }

    [Theory]
    [InlineData("TestString")]
    [InlineData(1231231)]
    public void IsEvalueateable_ResourceIsAnObject_ReturnTrue(object resource)
    {
        bool result = _rule.IsEvaluateable(resource);

        Assert.True(result);
    }
}