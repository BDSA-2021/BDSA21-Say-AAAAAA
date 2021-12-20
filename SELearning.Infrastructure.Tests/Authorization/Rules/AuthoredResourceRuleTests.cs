using System.Threading.Tasks;
using SELearning.Core.Collections;
using SELearning.Core.User;
using SELearning.Infrastructure.Authorization;
using SELearning.Infrastructure.Authorization.Rules;

namespace SELearning.Infrastructure.Tests.Authorization;

public class AuthoredResourceRuleTests
{
    private readonly AuthoredResourceRule _rule;
    private readonly IDynamicDictionary _context;

    public AuthoredResourceRuleTests()
    {
        _rule = new AuthoredResourceRule();
        _context = new DynamicDictionary();

        _context.Set<string>("UserId", "ABC");
    }

    [Fact]
    public async Task IsAllowed_ResourceHasTheSameUserId_ReturnTrue()
    {
        var user = new MockAuthoredResource(new UserDTO("ABC", "Albert"));

        bool result = await _rule.IsAllowed(_context, Permission.Rate, user);

        Assert.True(result);
    }

    [Fact]
    public async Task IsAllowed_ResourceWithDifferentUserId_ReturnFalse()
    {
        var user = new MockAuthoredResource(new UserDTO("AAAAASDADSAD", "Albert"));

        bool result = await _rule.IsAllowed(_context, Permission.Rate, user);

        Assert.False(result);
    }

    [Fact]
    public void IsEvalueateable_ResourceThatImplementsRequiredType_ReturnTrue()
    {
        bool result = _rule.IsEvaluateable(new MockAuthoredResource(null!));

        Assert.True(result);
    }

    [Fact]
    public void IsEvalueateable_ResourceThatDoesNotImplementRequiredType_ReturnFalse()
    {
        bool result = _rule.IsEvaluateable(new AuthoredResourceRule());

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