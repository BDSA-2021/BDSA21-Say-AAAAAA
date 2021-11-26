namespace SELearning.Core;

public interface ICommentRepository
{
    Task<Option<CommentDTO>> GetAsync(int ID);
    Task<CommentDTO> CreateAsync(int contentID, CommentDTO comment);
    Task<OperationResult> UpdateAsync(int ID, CommentDTO comment);
    Task<OperationResult> DeleteAsync(int ID);
}
