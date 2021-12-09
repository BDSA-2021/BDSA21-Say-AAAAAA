namespace SELearning.Core.Content;

public class ContentManager : IContentService
{
    private readonly IContentRepository _repository;

    public ContentManager(IContentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContentDto> AddContent(ContentCreateDto content)
    {
        return (await _repository.AddContent(content)).Item2;
    }

    public async Task<SectionDto> AddSection(SectionCreateDto section)
    {
        return (await _repository.AddSection(section)).Item2;
    }

    public async Task DecreaseContentRating(int id)
    {
        var content = await _repository.GetContent(id);

        if (content.IsNone)
        {
            throw new ContentNotFoundException(id);
        }

        ContentUpdateDto dto = new()
        {
            Title = content.Value.Title,
            Description = content.Value.Description,
            Rating = content.Value.Rating - 1,
            SectionId = content.Value.Section!.Id,
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

    public async Task DeleteSection(int id)
    {
        if (await _repository.DeleteSection(id) == OperationResult.NotFound)
        {
            throw new SectionNotFoundException(id);
        }
    }

    public async Task<IReadOnlyCollection<ContentDto>> GetContent()
    {
        var content = await _repository.GetContent();

        return content;
    }

    public async Task<ContentDto> GetContent(int id)
    {
        var content = await _repository.GetContent(id);

        if (content.IsNone)
        {
            throw new ContentNotFoundException(id);
        }

        return content.Value;
    }

    public async Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id)
    {
        var content = await _repository.GetContentInSection(id);

        if (content == null)
        {
            throw new SectionNotFoundException(id);
        }

        return content;
    }

    public async Task IncreaseContentRating(int id)
    {

        var content = await _repository.GetContent(id);

        if (content.IsNone)
        {
            throw new ContentNotFoundException(id);
        }

        ContentUpdateDto dto = new()
        {
            Title = content.Value.Title,
            Description = content.Value.Description,
            Rating = content.Value.Rating + 1,
            SectionId = content.Value.Section!.Id,
            VideoLink = content.Value.VideoLink
        };

        await UpdateContent(id, dto);
    }

    public async Task<IReadOnlyCollection<SectionDto>> GetSections()
    {
        var section = await _repository.GetSections();

        return section;
    }

    public async Task<SectionDto> GetSection(int id)
    {
        var section = await _repository.GetSection(id);

        if (section.IsNone)
        {
            throw new SectionNotFoundException(id);
        }

        return section.Value;
    }

    public async Task UpdateContent(int id, ContentUpdateDto content)
    {
        if (await _repository.UpdateContent(id, content) == OperationResult.NotFound)
        {
            throw new ContentNotFoundException(id);
        }
    }

    public async Task UpdateSection(int id, SectionUpdateDto section)
    {
        if (await _repository.UpdateSection(id, section) == OperationResult.NotFound)
        {
            throw new SectionNotFoundException(id);
        }
    }

    public async Task<IEnumerable<ContentDto>> GetContentByAuthor(string userId)
        => await _repository.GetContentByAuthor(userId);
}
