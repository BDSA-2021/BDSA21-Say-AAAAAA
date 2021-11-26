namespace SELearning.Infrastructure;
public class ContentRepository : IContentRepository
{
    private readonly IContentContext _context;

    public ContentRepository(IContentContext context)
    {
        _context = context;
    }
    public async Task<(OperationResult, ContentDto)> CreateAsync(ContentCreateDto content)
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

        return (OperationResult.Created, new ContentDto(entity.Id, entity.Section, entity.Author, entity.Title, entity.Description, entity.VideoLink, entity.Rating));
    }

    public void AddSection(Section section)
    {
        throw new NotImplementedException();
    }

    public void DeleteContent(string id)
    {
        throw new NotImplementedException();
    }

    public void DeleteSection(string id)
    {
        throw new NotImplementedException();
    }

    public void EditSection(string id, Section section)
    {
        throw new NotImplementedException();
    }

    public List<Content> GetContent()
    {
        throw new NotImplementedException();
    }

    public Content ReadAsync(string id)
    {
        throw new NotImplementedException();
    }

    public List<Content> GetContentInSection(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<OperationResult> UpdateAsync(int id, ContentUpdateDto content)
    {
        // var entity = await _context.Content.Include(c => c.Powers).FirstOrDefaultAsync(c => c.Id == character.Id);
        var entity = await _context.Content.FirstOrDefaultAsync(c => c.Id == id);

        if (entity == null)
        {
            return OperationResult.NotFound;
        }

        entity.Id = content.Id;
        entity.Author = content.Author;
        entity.Title = content.Title;
        entity.Description = content.Description;
        entity.Section = content.Section;
        entity.Rating = content.Rating;

        await _context.SaveChangesAsync();

        return OperationResult.Updated;
    }
}