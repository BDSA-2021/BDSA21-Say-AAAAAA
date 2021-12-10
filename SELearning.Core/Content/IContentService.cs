namespace SELearning.Core.Content;

public interface IContentService
{
    // Content
    public Task<ContentDto> AddContent(ContentCreateDto content);
    public Task UpdateContent(int id, ContentUpdateDto content);
    public Task DeleteContent(int id);
    public Task<IReadOnlyCollection<ContentDto>> GetContent();
    public Task<ContentDto> GetContent(int id);
    public Task IncreaseContentRating(int id);
    public Task DecreaseContentRating(int id);
    public Task<IEnumerable<ContentDto>> GetContentByAuthor(string userId);
}
