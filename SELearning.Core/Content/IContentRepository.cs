namespace SELearning.Core.Content;

public interface IContentRepository
{
    // Content
    public Task<(OperationResult, ContentDto)> AddContent(ContentCreateDto content);
    public Task<OperationResult> UpdateContent(int id, ContentUpdateDto content);
    public Task<OperationResult> DeleteContent(int id);
    public Task<IReadOnlyCollection<ContentDto>> GetContent();
    public Task<Option<ContentDto>> GetContent(int id);
    public Task<IEnumerable<ContentDto>> GetContentByAuthor(string userId);

    // Section
    public Task<(OperationResult, SectionDto)> AddSection(SectionCreateDto section);
    public Task<OperationResult> UpdateSection(int id, SectionUpdateDto section);
    public Task<OperationResult> DeleteSection(int id);
    public Task<IReadOnlyCollection<SectionDto>> GetSections();
    public Task<Option<SectionDto>> GetSection(int id);
    public Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id);
}
