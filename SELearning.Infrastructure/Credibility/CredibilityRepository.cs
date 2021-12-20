using SELearning.Core.Credibility;

namespace SELearning.Infrastructure.Credibility;

public class CredibilityRepository : ICredibilityRepository
{
    private readonly ICommentService _commentService;
    private readonly IContentService _contentService;

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
