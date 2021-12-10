using SELearning.Core.Section;

namespace SELearning.Infrastructure;

public class SectionRepository : ISectionRepository
{
    private readonly ISELearningContext _context;

    public SectionRepository(ISELearningContext context)
    {
        _context = context;
    }

    public async Task<(OperationResult, SectionDto)> AddSection(SectionCreateDto section)
    {
        var entity = new Section
        {
            Title = section.Title,
            Description = section.Description,
            Content = new List<Content>(),
        };

        _context.Section.Add(entity);

        await _context.SaveChangesAsync();

        return (OperationResult.Created, ConvertToSectionDTO(entity));
    }

    public async Task<OperationResult> UpdateSection(int id, SectionUpdateDto section)
    {
        var entity = await _context.Section.FirstOrDefaultAsync(s => s.Id == id);

        if (entity == null)
        {
            return OperationResult.NotFound;
        }

        entity.Title = section.Title;
        entity.Description = section.Description;
        entity.Content = new List<Content>();

        await _context.SaveChangesAsync();

        return OperationResult.Updated;
    }

    public async Task<OperationResult> DeleteSection(int id)
    {
        var entity = await _context.Section.FindAsync(id);

        if (entity == null)
        {
            return OperationResult.NotFound;
        }

        _context.Section.Remove(entity);
        await _context.SaveChangesAsync();

        return OperationResult.Deleted;
    }

    public async Task<IReadOnlyCollection<SectionDto>> GetSections() =>
        (await _context.Section
                       .Select(s => ConvertToSectionDTO(s))
                       .ToListAsync())
                       .AsReadOnly();

    public async Task<Option<SectionDto>> GetSection(int id)
    {
        var section = from s in _context.Section
                      where s.Id == id
                      select ConvertToSectionDTO(s);

        return await section.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id)
    {
        var section = _context.Section.Single(s => s.Id == id);
        var content = _context.Content
            .Include(x => x.Section)
            .Include(x => x.Author)
            .Where(x => x.Section == section)
            .Select(c => ContentRepository.ConvertToContentDTO(c));

        return (await content.ToListAsync()).AsReadOnly();
    }

    private static SectionDto ConvertToSectionDTO(Section s)
    {
        return new SectionDto
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
        };
    }
}
