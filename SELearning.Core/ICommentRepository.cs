namespace SELearning.Core;

public interface ICommentRepository
{
    Task<Option<CommentDTO>> GetAsync(int ID);
    Task<IReadOnlyCollection<CommentDTO>> GetAsyncByContentID(int contentID);
    Task<(OperationResult, CommentDTO)> CreateAsync(int contentID, CommentDTO comment);
    Task<(OperationResult, CommentDTO)> UpdateAsync(int ID, CommentDTO comment);
    Task<OperationResult> DeleteAsync(int ID);
}
