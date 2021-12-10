namespace SELearning.Infrastructure;

public class ContentRepository : IContentRepository
{
    private readonly ISELearningContext _context;

    public ContentRepository(ISELearningContext context)
    {
        _context = context;
    }

    public async Task<(OperationResult, ContentDto)> AddContent(ContentCreateDto content)
    {
        var section = await _context.Section.FindAsync(content.SectionId);

        var entity = new Content(
            content.Title,
            content.Description,
            content.VideoLink,
            null,
            content.Author,
            section
        );

        _context.Content.Add(entity);

        await _context.SaveChangesAsync();

        var contentDto = ConvertToContentDTO(entity);

        return (OperationResult.Created, contentDto);
    }

    public async Task<OperationResult> UpdateContent(int id, ContentUpdateDto content)
    {
        var entity = await _context.Content.FirstOrDefaultAsync(c => c.Id == id);

        if (entity == null)
        {
            return OperationResult.NotFound;
        }

        entity.Title = content.Title;
        entity.Description = content.Description;
        entity.VideoLink = content.VideoLink;
        entity.Rating = content.Rating;

        await _context.SaveChangesAsync();

        return OperationResult.Updated;
    }

    public async Task<Option<ContentDto>> GetContent(int contentId)
    {
        var content = _context.Content
            .Include(x => x.Section)
            .Include(x => x.Author)
            .Where(x => x.Id == contentId)
            .Select(c => ConvertToContentDTO(c));

        return await content.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<ContentDto>> GetContent() =>
        (await _context.Content
                       .Include(x => x.Section)
                       .Include(x => x.Author)
                       .Select(c => ConvertToContentDTO(c))
                       .ToListAsync())
                       .AsReadOnly();

    public async Task<OperationResult> DeleteContent(int contentId)
    {
        var entity = await _context.Content.FindAsync(contentId);

        if (entity == null)
        {
            return OperationResult.NotFound;
        }

        _context.Content.Remove(entity);
        await _context.SaveChangesAsync();

        return OperationResult.Deleted;
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

        var sectionDto = new SectionDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
        };

        return (OperationResult.Created, sectionDto);
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
                       .Select(s => new SectionDto
                       {
                           Id = s.Id,
                           Title = s.Title,
                           Description = s.Description,
                       })
                       .ToListAsync())
                       .AsReadOnly();

    public async Task<Option<SectionDto>> GetSection(int id)
    {
        var section = from s in _context.Section
                      where s.Id == id
                      select new SectionDto
                      {
                          Id = s.Id,
                          Title = s.Title,
                          Description = s.Description,
                      };

        return await section.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id)
    {
        var section = _context.Section.Single(s => s.Id == id);
        var content = _context.Content
            .Include(x => x.Section)
            .Include(x => x.Author)
            .Where(x => x.Section == section)
            .Select(c => ConvertToContentDTO(c));

        return (await content.ToListAsync()).AsReadOnly();
    }

    public async Task<IEnumerable<ContentDto>> GetContentByAuthor(string userId)
    {
        var content = _context.Content
            .Include(x => x.Section)
            .Include(x => x.Author)
            .Where(x => x.Author.Id == userId)
            .Select(c => ConvertToContentDTO(c));

        return (await content.ToListAsync()).AsReadOnly();
    }

    private ContentDto ConvertToContentDTO(Content c)
    {
        return new ContentDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            VideoLink = c.VideoLink,
            Rating = c.Rating,
            Author = c.Author,
            Section = new Section
            {
                Id = c.Section.Id,
                Title = c.Section.Title,
                Description = c.Section.Description
            }
        };
    }
}

