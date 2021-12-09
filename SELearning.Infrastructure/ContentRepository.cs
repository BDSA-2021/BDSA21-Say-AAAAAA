using SELearning.Core.Section;

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
        var entity = new Content(
            content.Title,
            content.Description,
            content.VideoLink,
            null,
            content.Author,
            content.Section
        );

        _context.Content.Add(entity);

        await _context.SaveChangesAsync();

        var contentDto = new ContentDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            VideoLink = entity.VideoLink,
            Rating = entity.Rating,
            Author = entity.Author,
            Section = entity.Section
        };

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
            .Select(c => new ContentDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                VideoLink = c.VideoLink,
                Rating = c.Rating,
                Author = c.Author,
                Section = c.Section
            }
            );

        return await content.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<ContentDto>> GetContent() =>
        (await _context.Content
                       .Include(x => x.Section)
                       .Include(x => x.Author)
                       .Select(c => new ContentDto
                       {
                           Id = c.Id,
                           Title = c.Title,
                           Section = c.Section,
                           Author = c.Author,
                           Description = c.Description,
                           VideoLink = c.VideoLink,
                           Rating = c.Rating
                       })
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
    
    public async Task<IEnumerable<ContentDto>> GetContentByAuthor(string userId)
    {
        var content = _context.Content
            .Include(x => x.Section)
            .Include(x => x.Author)
            .Where(x => x.Author.Id == userId)
            .Select(c => new ContentDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                VideoLink = c.VideoLink,
                Rating = c.Rating,
                Author = c.Author,
                Section = c.Section
            }
            );

        return (await content.ToListAsync()).AsReadOnly();
    }
}
