namespace SELearning.Core.Content;
public interface IContentRepository
{
    public Task<(OperationResult, ContentDto)> CreateAsync(ContentCreateDto content);
    public Task<OperationResult> UpdateAsync(int id, ContentUpdateDto content);
    public void DeleteContent(string id);
    public void AddSection(Section section);
    public void EditSection(string id, Section section);
    public void DeleteSection(string id);
    public List<Content> GetContent();
    public Content ReadAsync(string id);
    public List<Content> GetContentInSection(string id);

}