namespace SELearning.Core.Content;

public interface ISectionService
{
    // Section
    public Task<SectionDto> AddSection(SectionCreateDto section);
    public Task DeleteSection(int id);
    public Task UpdateSection(int id, SectionUpdateDto section);
    public Task<IReadOnlyCollection<SectionDto>> GetSections();
    public Task<SectionDto> GetSection(int id);
    public Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id);
}
