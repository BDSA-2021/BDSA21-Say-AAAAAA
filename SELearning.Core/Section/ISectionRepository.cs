namespace SELearning.Core.Content;

public interface ISectionRepository
{
    // Section
    public Task<(OperationResult, SectionDto)> AddSection(SectionCreateDto section);
    public Task<OperationResult> UpdateSection(int id, SectionUpdateDto section);
    public Task<OperationResult> DeleteSection(int id);
    public Task<IReadOnlyCollection<SectionDto>> GetSections();
    public Task<Option<SectionDto>> GetSection(int id);
    public Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id);
}
