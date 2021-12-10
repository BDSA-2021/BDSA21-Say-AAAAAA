using SELearning.Core.Content;

namespace SELearning.Core.Section;

public class SectionManager : ISectionService
{
    private readonly ISectionRepository _repository;

    public SectionManager(ISectionRepository repository)
    {
        _repository = repository;
    }

    public async Task<SectionDto> AddSection(SectionCreateDto section)
    {
        return (await _repository.AddSection(section)).Item2;
    }

    public async Task DeleteSection(int id)
    {
        if (await _repository.DeleteSection(id) == OperationResult.NotFound)
        {
            throw new SectionNotFoundException(id);
        }
    }

    public async Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id)
    {
        var content = await _repository.GetContentInSection(id);

        if (content == null)
        {
            throw new SectionNotFoundException(id);
        }

        return content;
    }

    public async Task<IReadOnlyCollection<SectionDto>> GetSections()
    {
        var section = await _repository.GetSections();

        return section;
    }

    public async Task<SectionDto> GetSection(int id)
    {
        var section = await _repository.GetSection(id);

        if (section.IsNone)
        {
            throw new SectionNotFoundException(id);
        }

        return section.Value;
    }

    public async Task UpdateSection(int id, SectionUpdateDto section)
    {
        if (await _repository.UpdateSection(id, section) == OperationResult.NotFound)
        {
            throw new SectionNotFoundException(id);
        }
    }
}
