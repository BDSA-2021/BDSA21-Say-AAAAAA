namespace SELearning.Core;

public interface IContentRepository
{
    Task<ContentDTO> GetAsync(int ID);
    Task<ContentDTO> CreateAsync(ContentDTO content);
    Task<ContentDTO> UpdateAsync(int ID, ContentDTO content);
    Task<ContentDTO> DeleteAsync(int ID);
}
