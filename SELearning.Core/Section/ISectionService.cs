using SELearning.Core.Content;

namespace SELearning.Core.Section;

public interface ISectionService
{
    public Task<SectionDTO> AddSection(SectionCreateDTO section);
    public Task DeleteSection(int id);
    public Task UpdateSection(int id, SectionUpdateDTO section);
    public Task<IReadOnlyCollection<SectionDTO>> GetSections();
    public Task<SectionDTO> GetSection(int id);
    public Task<IReadOnlyCollection<ContentDTO>> GetContentInSection(int id);
}
