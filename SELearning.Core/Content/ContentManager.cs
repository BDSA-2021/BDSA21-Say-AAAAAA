namespace SELearning.Core.Content;
public class ContentManager : IContentService
{

    private IContentRepository _repository;

    public ContentManager(IContentRepository repository)
    {
        _repository = repository;
    }

    public void DecreaseContentRating(int id)
    {
        throw new NotImplementedException();
    }

    public void IncreaseContentRating(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<OperationResult> DeleteContent(int id) => await _repository.DeleteContent(id);

    public async Task<(OperationResult, ContentDto)> AddContent(ContentCreateDto content) => await _repository.AddContent(content);

    public async Task<(OperationResult, SectionDto)> AddSection(SectionCreateDto section) => await _repository.AddSection(section);

    public async Task<OperationResult> DeleteSection(int id) => await _repository.DeleteSection(id);

    public async Task<IReadOnlyCollection<ContentDto>> GetContent() => await _repository.GetContent();

    public async Task<Option<ContentDto>> GetContent(int id) => await _repository.GetContent(id);

    public async Task<IReadOnlyCollection<ContentDto>> GetContentInSection(int id) => await _repository.GetContentInSection(id);



    public async Task<IReadOnlyCollection<SectionDto>> ReadSectionAsync() => await _repository.ReadSectionAsync();

    public async Task<Option<SectionDto>> ReadSectionAsync(int id) => await _repository.ReadSectionAsync(id);

    public async Task<OperationResult> UpdateContent(int id, ContentUpdateDto content) => await _repository.UpdateContent(id, content);

    public async Task<OperationResult> UpdateSection(int id, SectionUpdateDto section) => await _repository.UpdateSection(id, section);
}