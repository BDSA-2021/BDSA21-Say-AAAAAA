namespace SELearning.Core.Comment;

public interface ICommentRepository
{
    Task<(OperationResult, CommentDetailsDTO)> AddComment(CommentCreateDTO cmt);
    Task<(OperationResult, CommentDetailsDTO?)> UpdateComment(int id, CommentUpdateDTO cmt);
    Task<OperationResult> RemoveComment(int id);
    Task<Option<CommentDetailsDTO>> GetCommentByCommentId(int commentId);
    Task<(IEnumerable<CommentDetailsDTO>?, OperationResult)> GetCommentsByContentId(int contentId);
    Task<(IEnumerable<CommentDetailsDTO>, OperationResult)> GetCommentsByAuthor(string userId);
}
