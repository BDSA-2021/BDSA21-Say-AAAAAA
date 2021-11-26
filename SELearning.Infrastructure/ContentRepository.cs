namespace SELearning.Infrastructure;
public class ContentRepository : IContentRepository
{
    private readonly IContentContext _context;

    public ContentRepository(IContentContext context)
    {
        _context = context;
    }
    public async Task<ContentDto> CreateAsync(ContentCreateDto content)
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

        return (new ContentDto(entity.Id, entity.Section, entity.Author, entity.Title, entity.Description, entity.VideoLink, entity.Rating));
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

    public Content GetContent(string id)
    {
        throw new NotImplementedException();
    }

    public List<Content> GetContentInSection(string id)
    {
        throw new NotImplementedException();
    }

    public void UpdateContent(string id, Content content)
    {
        throw new NotImplementedException();
    }
}