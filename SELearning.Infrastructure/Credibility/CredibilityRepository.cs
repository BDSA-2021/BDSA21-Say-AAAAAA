using SELearning.Core.Comment;
using SELearning.Core.Content;

namespace SELearning.Infrastructure.Credibility;

public class CredibilityRepository : ICredibilityRepository
{
    ICommentService _commentService;
    IContentService _contentService;

    public CredibilityRepository(ICommentService commentService, IContentService contentService)
    {
        _commentService = commentService;
        _contentService = contentService;
    }

    public async Task<int> GetCommentCredibilityScore(string userId)
        => (await _commentService.GetCommentsByAuthor(userId)).Sum(x => x.Rating);

    public async Task<int> GetContentCredibilityScore(string userId)
        => (await _contentService.GetContentByAuthor(userId)).Sum(x => x.Rating);
}
