using SELearning.Core.Section;

namespace SELearning.Infrastructure.Content;

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
        var author = await _context.Users.FindAsync(content.Author.Id);

        var entity = new Content(
            content.Title,
            content.Description,
            content.VideoLink,
            null,
            author,
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

    public async Task<IEnumerable<ContentDto>> GetContentByAuthor(string userId)
    {
        var content = _context.Content
            .Include(x => x.Section)
            .Include(x => x.Author)
            .Where(x => x.Author.Id == userId)
            .Select(c => ConvertToContentDTO(c));

        return (await content.ToListAsync()).AsReadOnly();
    }

    public static ContentDto ConvertToContentDTO(Content c)
    {
        return new ContentDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            VideoLink = c.VideoLink,
            Rating = c.Rating,
            Author = new UserDTO(c.Author.Id, c.Author.Name),
            Section = new SectionDto
            {
                Id = c.Section.Id,
                Title = c.Section.Title,
                Description = c.Section.Description
            }
        };
    }
}

