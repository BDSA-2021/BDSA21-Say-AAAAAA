using SELearning.Core.Comment;

namespace SELearning.Infrastructure
{
    public interface ICommentRepository
    {
        Task<(OperationResult, CommentDetailsDTO)> AddComment(CommentCreateDTO cmt);
        Task<(OperationResult, CommentDetailsDTO?)> UpdateComment(int Id, CommentUpdateDTO cmt);
        Task<OperationResult> RemoveComment(int Id);
        Task<Option<CommentDetailsDTO>> GetCommentByCommentId(int commentId);
        Task<(IEnumerable<CommentDetailsDTO>?, OperationResult)> GetCommentsByContentId(int contentId);
        Task<(IEnumerable<CommentDetailsDTO>, OperationResult)> GetCommentsByAuthor(string userId);
    }
}
