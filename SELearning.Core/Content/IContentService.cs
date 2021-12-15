namespace SELearning.Core.Content;

public interface IContentService
{
    // Content
    public Task<ContentDTO> AddContent(ContentCreateDto content);
    public Task UpdateContent(int id, ContentUpdateDTO content);
    public Task DeleteContent(int id);
    public Task<IReadOnlyCollection<ContentDTO>> GetContent();
    public Task<ContentDTO> GetContent(int id);
    public Task IncreaseContentRating(int id);
    public Task DecreaseContentRating(int id);
    public Task<IEnumerable<ContentDTO>> GetContentByAuthor(string userId);
}
