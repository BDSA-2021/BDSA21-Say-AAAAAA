namespace SELearning.Infrastructure;
public class ContentRepository : IContentRepository
{
    private readonly IContentContext _context;

    public ContentRepository(IContentContext context)
    {
        _context = context;
    }
    public void AddContent(Content content)
    {
        throw new NotImplementedException();
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