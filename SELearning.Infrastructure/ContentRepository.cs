namespace SELearning.Infrastructure;
public class ContentRepository : IContentRepository
{
    private readonly IContentContext _context;

    public ContentRepository(IContentContext context)
    {
        _context = context;
    }
    public async Task<(OperationResult, ContentDto)> CreateContentAsync(ContentCreateDto content)
    {
        var entity = new Content {
           Section = content.Section,
           Author = content.Author,
           Title = content.Title,
           Description = content.Description,
           VideoLink = content.VideoLink,
           Rating = content.Rating
        };

        _context.Content.Add(entity);

        await _context.SaveChangesAsync();

        var contentDto = new ContentDto {
            Id = entity.Id,
            Section = entity.Section,
            Author = entity.Author,
            Title = entity.Title,
            Description = entity.Description,
            VideoLink = entity.VideoLink,
            Rating = entity.Rating
        };

        return (OperationResult.Created, contentDto);
    }

    public async Task<OperationResult> UpdateContentAsync(int id, ContentUpdateDto content)
    {
        var entity = await _context.Content.FirstOrDefaultAsync(c => c.Id == id);

        if (entity == null)
        {
            return OperationResult.NotFound;
        }

        entity.Title = content.Title;
        entity.Description = content.Description;
        entity.Section = content.Section;

        await _context.SaveChangesAsync();

        return OperationResult.Updated;
    }

    public async Task<Option<ContentDto>> ReadContentAsync(int contentId)
    {
        var content = from c in _context.Content
                         where c.Id == contentId
                         select new ContentDto {
                             Id = c.Id,
                             Author = c.Author,
                             Title = c.Title,
                             Description = c.Description,
                             Section = c.Section,
                             VideoLink = c.VideoLink,
                             Rating = c.Rating
                         };

        return await content.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<ContentDto>> ReadContentAsync() =>
        (await _context.Content
                       .Select(c => new ContentDto {
                           Id  = c.Id,  Section = c.Section, Author = c.Author, Title = c.Title, Description = c.Description, VideoLink = c.VideoLink, Rating = c.Rating
                           })
                       .ToListAsync())
                       .AsReadOnly();

    public async Task<OperationResult> DeleteContentAsync(int contentId)
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

    public List<Content> GetContent()
    {
        throw new NotImplementedException();
    }

    public async Task<(OperationResult, SectionDto)> CreateSectionAsync(SectionCreateDto section)
    {
        var entity = new Section {
           Title = section.Title,
           Description = section.Description,
           Content = section.Content,
        };

        _context.Section.Add(entity);

        await _context.SaveChangesAsync();

        var contentDto = new SectionDto {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Content = entity.Content
        };

        return (OperationResult.Created, contentDto);
    }

    public async Task<OperationResult> UpdateSectionAsync(int id, SectionUpdateDto section)
    {
        var entity = await _context.Section.FirstOrDefaultAsync(s => s.Id == id);

        if (entity == null)
        {
            return OperationResult.NotFound;
        }

        entity.Title = section.Title;
        entity.Description = section.Description;
        entity.Content = section.Content;

        await _context.SaveChangesAsync();

        return OperationResult.Updated;
    }

    public async Task<OperationResult> DeleteSectionAsync(int id)
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

    public List<Content> GetContentInSection(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<SectionDto>> ReadSectionAsync() =>
        (await _context.Section
                       .Select(s => new SectionDto {
                           Id = s.Id, Title = s.Title, Description = s.Description, Content = s.Content
                           })
                       .ToListAsync())
                       .AsReadOnly();

    public async Task<Option<SectionDto>> ReadSectionAsync(int id)
    {
        var section = from s in _context.Section
                         where s.Id == id
                         select new SectionDto {
                             Id = s.Id,
                             Title = s.Title,
                             Description = s.Description,
                             Content = s.Content
                         };

        return await section.FirstOrDefaultAsync();
    }
}