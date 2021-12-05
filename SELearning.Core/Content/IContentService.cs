namespace SELearning.Core.Content;
interface IContentService
{
    public Task IncreaseContentRatingAsync(int id);
    public Task DecreaseContentRating(int id);


    // Content
    public Task AddContent(ContentCreateDto content);
    public Task UpdateContentAsync(int id, ContentUpdateDto content);
    public Task DeleteContent(int id);
    public Task<IReadOnlyCollection<ContentDto>> GetContent();
    public Task<Option<ContentDto>> GetContent(int id);

    // Section
    public Task AddSection(SectionCreateDto section);
    public Task DeleteSection(int id);
    public Task UpdateSectionAsync(int id, SectionUpdateDto section);
    public Task<IReadOnlyCollection<SectionDto>> GetSections();
    public Task<Option<SectionDto>> GetSection(int id);
    public Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id);
}