using System.Threading.Tasks;
using SELearning.Core.Collections;
using SELearning.Core.User;
using SELearning.Infrastructure.Authorization.Rules;

namespace SELearning.Infrastructure.Tests.Authorization.Rules;

public class AuthoredResourceRuleTests
{
    private readonly AuthoredResourceRule _rule;
    private readonly IDynamicDictionary _context;

    public AuthoredResourceRuleTests()
    {
        _rule = new AuthoredResourceRule();
        _context = new DynamicDictionary();

        _context.Set<string>("UserId", "ABC");
        _context.Set("UserCredibilityScore", 500);
        _context.Set<IReadOnlyDictionary<Permission, int>>("RequiredCredibilityScores",
            new Dictionary<Permission, int>
            {
                {Permission.Rate, 1000}, {Permission.EditAnyComment, 1000}, {Permission.DeleteAnyComment, 500},
                {Permission.EditOwnComment, 499}, {Permission.DeleteOwnComment, 501}
            });
    }

    [Fact]
    public async Task IsAllowed_ResourceHasTheSameUserId_ReturnTrue()
    {
        var user = new MockAuthoredResource(new UserDTO("ABC", "Albert"));

        var result = await _rule.IsAllowed(_context, Permission.EditOwnComment, user);

        Assert.True(result);
    }

    [Theory]
    [InlineData(Permission.DeleteAnyComment, true)]
    [InlineData(Permission.EditAnyComment, false)]
    public async Task IsAllowed_AnyPermission_ReturnCorrectResult(Permission p, bool expectedResult)
    {
        var user = new MockAuthoredResource(new UserDTO("ABC", "Albert"));

        var result = await _rule.IsAllowed(_context, p, user);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task IsAllowed_ResourceWithSameUserIdButCredibilityNotHighEnough_ReturnFalse()
    {
        var user = new MockAuthoredResource(new UserDTO("ABC", "Albert"));

        var result = await _rule.IsAllowed(_context, Permission.DeleteOwnComment, user);

        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_ResourceWithDifferentUserId_ReturnFalse()
    {
        var user = new MockAuthoredResource(new UserDTO("AAAAASDADSAD", "Albert"));

        var result = await _rule.IsAllowed(_context, Permission.Rate, user);

        Assert.False(result);
    }

    [Fact]
    public void IsEvalueateable_ResourceThatImplementsRequiredType_ReturnTrue()
    {
        var result = _rule.IsEvaluateable(new MockAuthoredResource(null!));

        Assert.True(result);
    }

    [Fact]
    public void IsEvalueateable_ResourceThatDoesNotImplementRequiredType_ReturnFalse()
    {
        var result = _rule.IsEvaluateable(new AuthoredResourceRule());

        Assert.False(result);
    }
}

internal class MockAuthoredResource : IAuthored
{
    public UserDTO Author { get; }

    public MockAuthoredResource(UserDTO user)
    {
        Author = user;
    }
}
