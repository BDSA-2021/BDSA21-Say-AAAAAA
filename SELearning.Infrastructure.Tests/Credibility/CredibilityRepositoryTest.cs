using System;
using System.Linq;
using System.Threading.Tasks;
using SELearning.Infrastructure.Credibility;

namespace SELearning.Infrastructure.Tests.Credibility;

public class CredibilityRepositoryTest
{
    private const string UserId = "gandalf";

    [Theory]
    [InlineData(17, new[] { 12, 5 })]
    [InlineData(0, new int[] { })]
    [InlineData(12345679, new[] { 20, 9876543, 2109876, 359240 })]
    public async Task GetContentCredibilityScore_WithContent_ReturnsSum(int expectedSum, int[] ratings)
    {
        var contents = ratings.Select(r => new ContentDTO { Rating = r });
        var contentService = new Mock<IContentService>();
        contentService.Setup(m => m.GetContentByAuthor(UserId)).ReturnsAsync(contents);
        var commentService = new Mock<ICommentService>();

        var credibilityRepository = new CredibilityRepository(commentService.Object, contentService.Object);
        var contentCredibilityScore = await credibilityRepository.GetContentCredibilityScore(UserId);

        Assert.Equal(expectedSum, contentCredibilityScore);
    }

    [Theory]
    [InlineData(17, new[] { 12, 5 })]
    [InlineData(0, new int[] { })]
    [InlineData(12345679, new[] { 20, 9876543, 2109876, 359240 })]
    public async Task GetCommentCredibilityScore_WithComment_ReturnsSum(int expectedSum, int[] ratings)
    {
        var comments = ratings.Select(r =>
            new CommentDetailsDTO(new Core.User.UserDTO("Homer", "Homer"), "", -1, DateTime.Now, r, -1));
        var commentService = new Mock<ICommentService>();
        commentService.Setup(m => m.GetCommentsByAuthor(UserId)).ReturnsAsync(comments);
        var contentService = new Mock<IContentService>();

        var credibilityRepository = new CredibilityRepository(commentService.Object, contentService.Object);
        var contentCredibilityScore = await credibilityRepository.GetCommentCredibilityScore(UserId);

        Assert.Equal(expectedSum, contentCredibilityScore);
    }
}
