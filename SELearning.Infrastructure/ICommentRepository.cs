using SELearning.Core.Comment;

namespace SELearning.Infrastructure
{
    public interface ICommentRepository
    {
        Task<(OperationResult, CommentDetailsDTO)> AddComment(CommentCreateDTO cmt);
        Task<(OperationResult, CommentDetailsDTO?)> UpdateComment(int Id, CommentUpdateDTO cmt);
        Task<OperationResult> RemoveComment(int Id);
        Task<Option<Comment>> GetCommentByCommentId(int commentId);
        Task<(List<Comment>?, OperationResult)> GetCommentsByContentId(int contentId);
    }
}
