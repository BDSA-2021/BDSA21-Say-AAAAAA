namespace SELearning.Core.Content;

public interface IContentRepository
{
    public Task<(OperationResult, ContentDTO)> AddContent(ContentDTO content);
    public Task<OperationResult> UpdateContent(string id, ContentDTO content);
    public Task<OperationResult> DeleteContent(string id);
    public void AddSection(Section section);
    public void EditSection(string id, Section section);
    public void DeleteSection(string id);
    public List<Content> GetContent();
    public Task<Option<ContentDTO>> GetContent(string id);
    public List<Content> GetContentInSection(string id);
}
