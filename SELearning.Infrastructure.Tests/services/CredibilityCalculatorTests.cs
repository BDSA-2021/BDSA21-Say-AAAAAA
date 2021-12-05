using System.Security.Claims;

namespace SELearning.Infrastructure.Tests;

public class CredibilityCalculatorTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(69, 0, 69)]
    [InlineData(0, 420, 420)]
    [InlineData(123, 1214, 1337)]
    [InlineData(-1, -4, -5)]
    [InlineData(10, -13, -3)]
    [InlineData(-11, 11, 0)]
    async public void GetCredibilityScore_GivenUser_ReturnSumOfCommentAndContentCredibilityScore(int contentScore, int commentScore, int expectedScore)
    {
        var claimIdentity = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "userId") });
        var user = new ClaimsPrincipal(claimIdentity);

        var repository = new Mock<ICredibilityRepository>();
        repository.Setup(m => m.GetContentCredibilityScore("userId")).ReturnsAsync(contentScore);
        repository.Setup(m => m.GetCommentCredibilityScore("userId")).ReturnsAsync(commentScore);

        var service = new CredibilityCalculator(repository.Object);

        var credibilityScore = await service.GetCredibilityScore(user);

        Assert.Equal(expectedScore, credibilityScore);
    }
}