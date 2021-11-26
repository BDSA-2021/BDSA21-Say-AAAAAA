using SELearning.Core.Comment;
namespace SELearning.Infrastructure
{
    public interface ICommentRepository
    {
        Task<(OperationResult, CommentDetailsDTO)> AddComment(CommentDTO cmt);
        Task<(OperationResult,CommentDetailsDTO)> UpdateComment(int Id, CommentDTO cmt);
        Task<OperationResult> RemoveComment(int Id);

        Task<List<Comment>> GetCommentsByContentId(int contentId);
    }
}