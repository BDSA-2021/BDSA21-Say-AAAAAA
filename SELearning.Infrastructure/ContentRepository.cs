namespace SELearning.Infrastructure;
public class ContentRepository : IContentRepository
{
    private readonly IContentContext _context;

    public ContentRepository(IContentContext context)
    {
        _context = context;
    }

    public Task<(OperationResult, ContentDTO)> AddContent(ContentDTO content)
    {
        throw new NotImplementedException();
    }

    public void AddSection(Section section)
    {
        throw new NotImplementedException();
    }

    Task<OperationResult> IContentRepository.DeleteContent(string id)
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

    Task<Option<ContentDTO>> IContentRepository.GetContent(string id)
    {
        throw new NotImplementedException();
    }

    public List<Content> GetContentInSection(string id)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult> UpdateContent(string id, ContentDTO content)
    {
        throw new NotImplementedException();
    }
}