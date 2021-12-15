namespace SELearning.Core.Content;

public interface IContentRepository
{
    // Content
    public Task<(OperationResult, ContentDTO)> AddContent(ContentCreateDto content);
    public Task<OperationResult> UpdateContent(int id, ContentUpdateDTO content);
    public Task<OperationResult> DeleteContent(int id);
    public Task<IReadOnlyCollection<ContentDTO>> GetContent();
    public Task<Option<ContentDTO>> GetContent(int id);
    public Task<IEnumerable<ContentDTO>> GetContentByAuthor(string userId);
}
