namespace SELearning.Core.Content;

public class ContentManager : IContentService
{
    private readonly IContentRepository _repository;

    public ContentManager(IContentRepository repository)
    {
        _repository = repository;
    }

    public async Task AddContent(ContentCreateDto content)
    {
        if ((await _repository.AddContent(content)).Item1 != OperationResult.Created)
        {
            throw new Exception("The content was not created");
        }
    }

    public async Task AddSection(SectionCreateDto section)
    {
        if ((await _repository.AddSection(section)).Item1 != OperationResult.Created)
        {
            throw new Exception("The section was not created");
        }
    }

    public async Task DecreaseContentRating(int id)
    {
        var content = await _repository.GetContent(id);

        if (content.IsNone)
        {
            throw new Exception("The comment could not be found");
        }

        ContentUpdateDto dto = new()
        {
            Title = content.Value.Title,
            Description = content.Value.Description,
            Author = content.Value.Author,
            Rating = content.Value.Rating - 1,
            Section = content.Value.Section,
            VideoLink = content.Value.VideoLink
        };

        await UpdateContentAsync(id, dto);
    }

    public async Task DeleteContent(int id)
    {
        if (await _repository.DeleteContent(id) == OperationResult.NotFound)
        {
            throw new Exception("The section could not be found");
        }
    }

    public async Task DeleteSection(int id)
    {
        if (await _repository.DeleteSection(id) == OperationResult.NotFound)
        {
            throw new Exception("The section could not be found");
        }
    }

    public async Task<IReadOnlyCollection<ContentDto>> GetContent()
    {
        var content = await _repository.GetContent();

        if (content == null)
        {
            throw new Exception("Could not get all sections");
        }

        return content;
    }

    public async Task<ContentDto> GetContent(int id)
    {
        var content = await _repository.GetContent(id);

        if (content.IsNone)
        {
            throw new Exception("The comment could not be found");
        }

        return content.Value;
    }

    public async Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id)
    {
        var content = await _repository.GetContentInSection(id);

        if (content == null)
        {
            throw new Exception("Could not get content in section");
        }

        return content;
    }

    public async Task IncreaseContentRatingAsync(int id)
    {

        var content = await _repository.GetContent(id);

        if (content.IsNone)
        {
            throw new Exception("The comment could not be found");
        }

        ContentUpdateDto dto = new()
        {
            Title = content.Value.Title,
            Description = content.Value.Description,
            Author = content.Value.Author,
            Rating = content.Value.Rating + 1,
            Section = content.Value.Section,
            VideoLink = content.Value.VideoLink
        };

        await UpdateContentAsync(id, dto);
    }

    public async Task<IReadOnlyCollection<SectionDto>> GetSections()
    {
        var section = await _repository.GetSections();

        if (section == null)
        {
            throw new Exception("Could not get all sections");
        }

        return section;
    }

    public async Task<SectionDto> GetSection(int id)
    {
        var section = await _repository.GetSection(id);

        if (section.IsNone)
        {
            throw new Exception("The comment could not be found");
        }

        return section.Value;
    }

    public async Task UpdateContentAsync(int id, ContentUpdateDto content)
    {
        if (await _repository.UpdateContent(id, content) == OperationResult.NotFound)
        {
            throw new Exception("The content could not be found");
        }
    }

    public async Task UpdateSectionAsync(int id, SectionUpdateDto section)
    {
        if (await _repository.UpdateSection(id, section) == OperationResult.NotFound)
        {
            throw new Exception("The section could not be found");
        }
    }
}
