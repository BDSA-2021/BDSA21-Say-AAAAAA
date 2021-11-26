namespace SELearning.Core;

public interface IContentRepository
{
    Task<Option<ContentDTO>> GetAsync(int ID);
    Task<ContentDTO> CreateAsync(ContentDTO content);
    Task<OperationResult> UpdateAsync(int ID, ContentDTO content);
    Task<OperationResult> DeleteAsync(int ID);
}
