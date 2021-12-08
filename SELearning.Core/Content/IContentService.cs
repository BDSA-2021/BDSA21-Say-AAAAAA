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

    // Section
    public Task<SectionDto> AddSection(SectionCreateDto section);
    public Task DeleteSection(int id);
    public Task UpdateSection(int id, SectionUpdateDto section);
    public Task<IReadOnlyCollection<SectionDto>> GetSections();
    public Task<SectionDto> GetSection(int id);
    public Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id);
}
