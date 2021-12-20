using SELearning.Core.Content;

namespace SELearning.Core.Section;

public interface ISectionRepository
{
    public Task<(OperationResult, SectionDTO)> AddSection(SectionCreateDTO section);
    public Task<OperationResult> UpdateSection(int id, SectionUpdateDTO section);
    public Task<OperationResult> DeleteSection(int id);
    public Task<IReadOnlyCollection<SectionDTO>> GetSections();
    public Task<Option<SectionDTO>> GetSection(int id);
    public Task<IReadOnlyCollection<ContentDTO>> GetContentInSection(int id);
}
