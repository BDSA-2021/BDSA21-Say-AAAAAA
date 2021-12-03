namespace SELearning.Core.Content;
public interface IContentRepository
{
    // Content
    public Task<(OperationResult, ContentDto)> AddContent(ContentCreateDto content);
    public Task<OperationResult> UpdateContent(int id, ContentUpdateDto content);
    public Task<OperationResult> DeleteContent(int id);
    public Task<IReadOnlyCollection<ContentDto>> GetContent();
    public Task<Option<ContentDto>> GetContent(int id);

    // Section
    public Task<(OperationResult, SectionDto)> AddSection(SectionCreateDto section);
    public Task<OperationResult> UpdateSection(int id, SectionUpdateDto content);
    public Task<OperationResult> DeleteSection(int id);
    public Task<IReadOnlyCollection<SectionDto>> ReadSectionAsync();
    public Task<Option<SectionDto>> ReadSectionAsync(int id);
    public Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id);
    
}