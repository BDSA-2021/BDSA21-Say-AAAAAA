namespace SELearning.Core;

public interface IContentRepository
{
    Task<ContentDTO> ReadAsync(int ID);
}
