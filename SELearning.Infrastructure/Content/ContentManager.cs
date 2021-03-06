namespace SELearning.Infrastructure.Content;

public class ContentManager : IContentService
{
    private readonly IContentRepository _repository;

    public ContentManager(IContentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContentDTO> AddContent(ContentCreateDto content)
    {
        return (await _repository.AddContent(content)).Item2;
    }

    public async Task DecreaseContentRating(int id)
    {
        var content = await _repository.GetContent(id);

        if (content.IsNone)
        {
            throw new ContentNotFoundException(id);
        }

        ContentUpdateDTO dto = new()
        {
            Title = content.Value.Title,
            Description = content.Value.Description,
            Rating = content.Value.Rating - 1,
            VideoLink = content.Value.VideoLink
        };

        await UpdateContent(id, dto);
    }

    public async Task DeleteContent(int id)
    {
        if (await _repository.DeleteContent(id) == OperationResult.NotFound)
        {
            throw new ContentNotFoundException(id);
        }
    }

    public async Task<IReadOnlyCollection<ContentDTO>> GetContent()
    {
        var content = await _repository.GetContent();

        return content;
    }

    public async Task<ContentDTO> GetContent(int id)
    {
        var content = await _repository.GetContent(id);

        if (content.IsNone)
        {
            throw new ContentNotFoundException(id);
        }

        return content.Value;
    }

    public async Task IncreaseContentRating(int id)
    {
        var content = await _repository.GetContent(id);

        if (content.IsNone)
        {
            throw new ContentNotFoundException(id);
        }

        ContentUpdateDTO dto = new()
        {
            Title = content.Value.Title,
            Description = content.Value.Description,
            Rating = content.Value.Rating + 1,
            VideoLink = content.Value.VideoLink
        };

        await UpdateContent(id, dto);
    }

    public async Task UpdateContent(int id, ContentUpdateDTO content)
    {
        if (await _repository.UpdateContent(id, content) == OperationResult.NotFound)
        {
            throw new ContentNotFoundException(id);
        }
    }

    public async Task<IEnumerable<ContentDTO>> GetContentByAuthor(string userId)
        => await _repository.GetContentByAuthor(userId);
}
