namespace SELearning.Infrastructure.Content;

public class ContentRepository : IContentRepository
{
    private readonly ISELearningContext _context;

    public ContentRepository(ISELearningContext context)
    {
        _context = context;
    }

    public async Task<(OperationResult, ContentDTO)> AddContent(ContentCreateDto content)
    {
        var section = await _context.Section.FindAsync(content.SectionId);
        var author = await _context.Users.FirstOrDefaultAsync(c => c.Id == content.Author.Id);

        if (section == null || author == null)
        {
            return (OperationResult.NotFound, null!);
        }

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

        return (OperationResult.Created, entity.ToContentDTO());
    }

    public async Task<OperationResult> UpdateContent(int id, ContentUpdateDTO content)
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

    public async Task<Option<ContentDTO>> GetContent(int contentId)
    {
        var content = _context.Content
            .Include(x => x.Section)
            .Include(x => x.Author)
            .Where(x => x.Id == contentId)
            .Select(c => c.ToContentDTO());

        return await content.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<ContentDTO>> GetContent() =>
        (await _context.Content
            .Include(x => x.Section)
            .Include(x => x.Author)
            .Select(c => c.ToContentDTO())
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

    public async Task<IEnumerable<ContentDTO>> GetContentByAuthor(string userId)
    {
        var content = _context.Content
            .Include(x => x.Section)
            .Include(x => x.Author)
            .Where(x => x.Author.Id == userId)
            .Select(c => c.ToContentDTO());

        return (await content.ToListAsync()).AsReadOnly();
    }
}
