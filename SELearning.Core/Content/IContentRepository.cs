namespace SELearning.Core.Content;
public interface IContentRepository
{
    // Content
    public Task<(OperationResult, ContentDto)> CreateContentAsync(ContentCreateDto content);
    public Task<OperationResult> UpdateContentAsync(int id, ContentUpdateDto content);
    public Task<OperationResult> DeleteContentAsync(int id);
    public Task<IReadOnlyCollection<ContentDto>> ReadContentAsync();
    public Task<Option<ContentDto>> ReadContentAsync(int id);
    public List<Content> GetContent();

    // Section
    public Task<(OperationResult, SectionDto)> CreateSectionAsync(SectionCreateDto section);
    public Task<OperationResult> UpdateSectionAsync(int id, SectionUpdateDto content);
    public Task<OperationResult> DeleteSectionAsync(int id);
    public List<Content> GetContentInSection(int id);
    public Task<IReadOnlyCollection<SectionDto>> ReadSectionAsync();
    public Task<Option<SectionDto>> ReadSectionAsync(int id);
    
}