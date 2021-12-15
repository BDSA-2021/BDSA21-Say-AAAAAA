using System;
using System.Linq;
using System.Threading.Tasks;
using SELearning.Infrastructure.Credibility;

namespace SELearning.Infrastructure.Tests.Credibility;

public class CredibilityRepositoryTest
{
    string _userId = "gandalf";

    [Theory]
    [InlineData(17, new int[] { 12, 5 })]
    [InlineData(0, new int[] { })]
    [InlineData(12345679, new int[] { 20, 9876543, 2109876, 359240 })]
    public async Task GetContentCredibilityScore_WithContent_ReturnsSum(int expectedSum, int[] ratings)
    {
        var contents = ratings.Select(r => new ContentDto { Rating = r });
        var contentService = new Mock<IContentService>();
        contentService.Setup(m => m.GetContentByAuthor(_userId)).ReturnsAsync(contents);
        var commentService = new Mock<ICommentService>();

        var credibilityRepository = new CredibilityRepository(commentService.Object, contentService.Object);
        var contentCredibilityScore = await credibilityRepository.GetContentCredibilityScore(_userId);

        Assert.Equal(expectedSum, contentCredibilityScore);
    }

    [Theory]
    [InlineData(17, new int[] { 12, 5 })]
    [InlineData(0, new int[] { })]
    [InlineData(12345679, new int[] { 20, 9876543, 2109876, 359240 })]
    public async Task GetCommentCredibilityScore_WithComment_ReturnsSum(int expectedSum, int[] ratings)
    {
        var comments = ratings.Select(r => new CommentDetailsDTO(new Core.User.UserDTO("Homer", "Homer"), "", -1, DateTime.Now, r, -1));
        var commentService = new Mock<ICommentService>();
        commentService.Setup(m => m.GetCommentsByAuthor(_userId)).ReturnsAsync(comments);
        var contentService = new Mock<IContentService>();

        var credibilityRepository = new CredibilityRepository(commentService.Object, contentService.Object);
        var contentCredibilityScore = await credibilityRepository.GetCommentCredibilityScore(_userId);

        Assert.Equal(expectedSum, contentCredibilityScore);
    }
}