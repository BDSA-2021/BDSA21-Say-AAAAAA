namespace SELearning.Core.Content;
public interface IContentRepository
{
    public Task<ContentDto> CreateAsync(ContentCreateDto content);
    
    //TODO: Hvis du prøver at update et stykke content der ikke findes via ID så retuer NotFound. Ellers returner updated.
    public void UpdateContent(string id, Content content);
    public void DeleteContent(string id);
    public void AddSection(Section section);
    public void EditSection(string id, Section section);
    public void DeleteSection(string id);
    public List<Content> GetContent();
    public Content GetContent(string id);
    public List<Content> GetContentInSection(string id);

}